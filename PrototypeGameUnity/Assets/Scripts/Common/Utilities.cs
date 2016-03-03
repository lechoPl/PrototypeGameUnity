using System;

namespace Assets.Scripts.Common
{
    public static class Utilities
    {
        private static Random _rand;
        public static Random rand
        {
            get
            {
                if (_rand == null)
                {
                    _rand = new Random();
                }

                return _rand;
            }
        }
    }
}
