﻿using System;
using System.Collections.Generic;

namespace Model
{
    public class CompetitionData<T>
    {
        private List<T> _list = new List<T>();

        public void AddData(T data)
        {
            _list.Add(data);

        }
    }
}
