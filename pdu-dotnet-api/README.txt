Build and run C# bindings and examples

1. Windows

- open src/dotnet_json_rpc.sln in Visual Studio
- in Solution Explorer, right click on dotnet_json_rpc_examples and select "Set as Startup Project"
- press F5 to build and run

2. Linux

2.1 Prerequisites for Ubuntu:
- mono-xbuild
- mono-mcs
- libmono-system-data-datasetextensions4.0-cil

Install with:

sudo apt install mono-xbuild mono-mcs libmono-system-data-datasetextensions4.0-cil


2.2 Prerequisites for Fedora:
- mono-devel
- mono-data

Install with:

sudo dnf install mono-devel mono-data

2.3 Compilation

- start build:

cd examples/src
xbuild dotnet_json_rpc_examples.csproj

- run examples against PDU:

cd bin/Debug
mono examples.exe

