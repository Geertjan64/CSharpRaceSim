using System;
using System.Collections.Generic;

namespace Model
{
    public class CompetitionData<T> where T : ParticipantStatistic
    {
        private List<T> _list = new List<T>();

        public void AddData(T data)
        {
            _list.Add(data);

        }
    }
}
