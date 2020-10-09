using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticpant
    {
        private string _name;
        private IEquipment _equipment;
        private int _points;
        private TeamColors _teamColors;
        
        public string Name { get => _name; set => _name = value; }
        public int Points { get => _points; set => _points = value; }
        public TeamColors TeamColor { get => _teamColors; set => _teamColors = value; }
        public IEquipment Equipment { get => _equipment; set => _equipment = value; }

        public Driver( string name, int points , TeamColors colour)
        {
            Name = name;
            Points = points;
            TeamColor = colour;

            Equipment = new Car();

         
        }
      
       
    }
}
