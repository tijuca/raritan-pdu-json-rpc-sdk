// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Util {
    /**
     * Tracks an asynchronous activity.
     * 
     * <p>
     * Asynchronous object methods usually return this as a handle for the task that
     * they are going to perform. They also keep the handle themselves, and once
     * they're done, they call succeeded() or failed() on it, to make it go to
     * success/failure state.
     * </p>
     * 
     * <p>
     * Users of this can call addSuccessListener() or addFailureListener() at any
     * time, to be notified when this goes to or already is in success/failure
     * state. Notification will always happen asynchronously, i.e. via event loop,
     * and not via a direct recursive call from the add...() methods above.
     * </p>
     * 
     * Note that this is only <em>tracks</em> an activity, but does provide the
     * implementation itself. Compare to: {@link AsyncCommand}.
     */
    public class AsyncRequest {
        //
        // event delegates
        //
        public delegate void DoneHandler();

        public delegate void FailureHandler(Exception exception);

        public delegate void SuccessHandler(object data);

        //
        // event registrations
        //
        private event DoneHandler _Done;

        /**
         * Add a listener to notify when task has either succeeded or failed.
         *
         * If it already has failed, listener will still be notified.
         * Notification is guaranteed to happen asynchronously.
         */
        public event DoneHandler Done {
            add {
                if (IsDone) {
                    value();
                } else {
                    _Done += value;
                }
            }
            remove { _Done -= value; }
        }

        private event FailureHandler _Failure;

        /**
         * Add a listener to notify in case this task fails.
         *
         * If it already has failed, listener will still be notified.
         * Notification is guaranteed to happen asynchronously.
         */
        public event FailureHandler Failure {
            add {
                if (HasFailed) {
                    value(m_failureException);
                } else {
                    _Failure += value;
                }
            }
            remove { _Failure -= value; }
        }

        public event SuccessHandler _Success;

        /**
         * Add a listener to notify in case this task succeeds.
         *
         * If it already has succeeded, listener will still be notified.
         * Notification is guaranteed to happen asynchronously.
         */
        public event SuccessHandler Success {
            add {
                if (HasSucceeded) {
                    value(m_successData);
                } else {
                    _Success += value;
                }
            }
            remove { _Success -= value; }
        }

        //
        // Internal things
        //
        private const string CLASS_NAME = "AsyncRequest";

        private enum State {
            NEW,       // task just created - not running, yet
            STARTED,   // task is running
            SUCCESS,   // task succeeded - final state
            FAILURE,   // task failed - final state
        };

        private State m_state = State.NEW;
        private object m_successData;
        private Exception m_failureException;
        private string m_label;

        //
        // public API
        //
        /**
         * Constructor.
         * 
         * @param label  a (unique) string to associate with this instance - for debugging
         */
        public AsyncRequest(string label) {
            m_label = label;
        }

        /**
         * Inform this instance that the task it represents has started to run.
         *
         * @return this instance (for syntactic convenience)
         */
        public virtual AsyncRequest Started() {
            Debug.Assert(m_state == State.NEW);
            m_state = State.STARTED;
            return this;
        }

        /**
         * Inform this instance that the task it represents has succeeded.
         *
         * @param data  context data to pass to registered success listeners
         * @return this instance (for syntactic convenience)
         */
        public virtual AsyncRequest Succeeded(object data) {
            Debug.Assert(IsNew || HasStarted);
            m_state = State.SUCCESS;
            m_successData = data;

            if (_Success != null) _Success(data);
            if (_Done != null) _Done();
            return this;
        }

        /**
         * Inform this instance that the task it represents has failed.
         *
         * @param exc  exception to pass to registered failure listeners
         */
        public virtual AsyncRequest Failed(Exception exc) {
            Debug.Assert(IsNew || HasStarted);
            m_state = State.FAILURE;
            m_failureException = exc;

            if (_Failure != null) _Failure(exc);
            if (_Done != null) _Done();
            return this;
        }

        //
        // Properties
        //
        public bool IsNew {
            get { return m_state == State.NEW; }
        }

        public bool HasStarted {
            get { return m_state == State.STARTED; }
        }

        public bool HasFailed {
            get { return m_state == State.FAILURE; }
        }

        public bool HasSucceeded {
            get { return m_state == State.SUCCESS; }
        }

        public bool IsDone {
            get { return HasSucceeded || HasFailed; }
        }

        public string Label {
            get { return m_label; }
        }

        public object SuccessData {
            get { return m_successData; }
        }

        public Exception FailureException {
            get { return m_failureException; }
        }

        public override string ToString() {
            return CLASS_NAME + "@" + GetHashCode().ToString("X") +
                    "(label = '" + m_label + "', state = " + m_state + ")";
        }
    }
}
