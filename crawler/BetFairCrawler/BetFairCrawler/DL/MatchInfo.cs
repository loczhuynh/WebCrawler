using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFairCrawler.DL
{
    public class MatchInfo
    {
        private string _type;

        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        //specify the match name
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _time;

        public string time
        {
            get { return _time; }
            set { _time = value; }
        }
        private double _startHomeOdd;

        public double startHomeOdd
        {
            get { return _startHomeOdd; }
            set { _startHomeOdd = value; }
        }

        private double _curHomeOdd;

        public double currentHomeOdd
        {
            get { return _curHomeOdd; }
            set { _curHomeOdd = value; }
        }

        private double _startDrawOdd;

        public double startDrawOdd
        {
            get { return _startDrawOdd; }
            set { _startDrawOdd = value; }
        }

        private double _curDrawOdd;

        public double currentDrawOdd
        {
            get { return _curDrawOdd; }
            set { _curDrawOdd = value; }
        }

        private double _startAwayOdd;

        public double startAwayOdd
        {
            get { return _startAwayOdd; }
            set { _startAwayOdd = value; }
        }

        private double _curAwayOdd;

        public double currentAwayOdd
        {
            get { return _curAwayOdd; }
            set { _curAwayOdd = value; }
        }
        
        private decimal _totalMoney;

        public decimal totalMoney
        {
            get { return _totalMoney; }
            set { _totalMoney = value; }
        }

        private string _vIndex;
        public string vindex
        {
            get { return _vIndex; }
            set { _vIndex = value; }
        }
        
        private string _currentPick;

        public string currentPick
        {
            get { return _currentPick; }
            set { _currentPick = value; }
        }
        private int _result;

        public int result
        {
            get { return _result; }
            set { _result = value; }
        }

        private int _oddHomeType;

        public int oddHomeType
        {
            get { return _oddHomeType; }
            set { _oddHomeType = value; }
        }

        private int _oddDrawType;

        public int oddDrawType
        {
            get { return _oddDrawType; }
            set { _oddDrawType = value; }
        }

        private int _oddAwayType;

        public int oddAwayType
        {
            get { return _oddAwayType; }
            set { _oddAwayType = value; }
        }

        //override Equals
        public override bool Equals(object obj)
        {
            MatchInfo matchInput = (MatchInfo)obj;
            return (this.name == matchInput.name
                && this.time == matchInput.time
                && this.type == matchInput.type);
            
        }
    }
}
