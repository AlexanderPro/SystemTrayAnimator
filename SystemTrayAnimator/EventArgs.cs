﻿using System;

namespace SystemTrayAnimator
{
    [Serializable]
    public class EventArgs<T> : EventArgs
    {
        public T Entity { get; }

        public EventArgs(T entity)
        {
            Entity = entity;
        }
    }
}
