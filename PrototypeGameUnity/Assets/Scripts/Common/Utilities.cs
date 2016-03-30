using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class Utilities
    {
        public static readonly float eps = 0.00001f;

        private static System.Random _rand;
        public static System.Random rand
        {
            get
            {
                if (_rand == null)
                {
                    _rand = new System.Random();
                }

                return _rand;
            }
        }

        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }

            return null;
        }

        public static GameObject FindChildWithTag(this GameObject parent, string tag)
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.gameObject;
                }
            }

            return null;
        }
    }
}
