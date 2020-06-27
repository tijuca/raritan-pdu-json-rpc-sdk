# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC::Registry;

use strict;
use warnings;

use Raritan::RPC::Exception;

#
# Proxy or codec class registry: The 'classes' member is a triply-nested hash,
# indexed by base+major, minor and submajor.
#
# Note: Due to the dynamic nature of the Perl proxies the submajor number has
#       virtually no significance and is checked last.
#

sub new {
    my ($class) = @_;
    my $self = {
	classes => {}
    };
    bless $self, $class;
    return $self;
}

sub registerClass($$$$$$) {
    my ($self, $base, $major, $submajor, $minor, $class) = @_;
    my $base_major = "$base:$major";
    if (!defined $self->{'classes'}->{$base_major}) {
	$self->{'classes'}->{$base_major} = {};
    }
    if (!defined $self->{'classes'}->{$base_major}->{$minor}) {
	$self->{'classes'}->{$base_major}->{$minor} = {};
    }
    $self->{'classes'}->{$base_major}->{$minor}->{$submajor} = $class;
}

#
# Find the best matching class for the given type id.
#
# A class is considered compatible if its class name and major number
# match the given type id and its minor number is smaller or equal. The
# best match is the compatible class with the largest minor number.
#
sub findClass($$) {
    my ($self, $typeId) = @_;
    my ($base, $major, $submajor, $minor);
    if ($typeId =~ /^(.+):([0-9]+)\.([0-9]+)\.([0-9]+)$/) {
	($base, $major, $submajor, $minor) = ($1, $2, $3, $4);
    } elsif ($typeId =~ /^(.+):([0-9]+)\.([0-9]+)$/) {
	($base, $major, $submajor, $minor) = ($1, $2, 0, $3);
    } else {
	throw Raritan::RPC::JsonRpcSyntaxException("Invalid type id: $typeId");
    }

    my $classes = $self->{'classes'}->{"$base:$major"};
    if (!defined $classes) {
	return undef;
    }

    my $best_minor = undef;
    foreach my $pminor (keys %{$classes}) {
	if ($pminor <= $minor && (!defined $best_minor || $pminor >= $best_minor)) {
	    $best_minor = $pminor;
	}
    }
    if (!defined $best_minor) {
	return undef;
    }
    $classes = $classes->{$best_minor};

    my $class = $classes->{$submajor};
    if (!defined $class) {
	# No exact match for the requested submajor number; just pick one
	$class = (values(%{$classes}))[0];
    }

    my $tid = $class->typeId;
    if ($tid eq $typeId) {
	Raritan::RPC::debug("Found perfect match for $typeId\n");
    } else {
	Raritan::RPC::debug("Best match for $typeId: $tid\n");
    }
    return $class;
}

my $proxyRegistry = new Raritan::RPC::Registry();

sub registerProxyClass($$$$$) {
    my ($base, $major, $submajor, $minor, $class) = @_;
    $proxyRegistry->registerClass($base, $major, $submajor, $minor, $class);
}

sub findProxyClass($) {
    my ($typeId) = @_;
    return $proxyRegistry->findClass($typeId);
}

my $codecRegistry = new Raritan::RPC::Registry();

sub registerCodecClass($$$$$) {
    my ($base, $major, $submajor, $minor, $class) = @_;
    $codecRegistry->registerClass($base, $major, $submajor, $minor, $class);
}

sub findCodecClass($) {
    my ($typeId) = @_;
    return $codecRegistry->findClass($typeId);
}

1;
