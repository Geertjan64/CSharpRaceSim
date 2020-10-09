using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Car : IEquipment
    {
        private Random _rnd;


        private double _luckyCharm;
        private int _quality;
        private int _performance;
        private int _speed;
        private bool _isBroken;
        public int Quality { get => _quality; set => _quality = value; }
        public int Performance { get => _performance; set => _performance = value; }
        public int Speed { get => _speed ; set => _speed = value; }
        public bool IsBroken { get => _isBroken; set => _isBroken = value; }

        public int  getCarVelocity()
        {
            return (int) Math.Ceiling( ( (Performance / (_luckyCharm  * 0.04)) * Speed ) / (Quality * 3));
        }
        public Car()
        {
            _rnd = new Random();

            _luckyCharm = _rnd.NextDouble();
            Performance = _rnd.Next(10, 15);
            Quality = _rnd.Next(1, 10);
            Speed = _rnd.Next(10, 30 ) ;
        }
    }
}
