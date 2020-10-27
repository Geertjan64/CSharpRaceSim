using System;
using System.Collections.Generic;

namespace Model
{
    public class CompetitionData<T> where T : class, ParticipantStatistic
    {
        private List<T> _list = new List<T>();

        public void AddData(T data)
        {
            data.Add<T>(_list);
        }
    }
}
