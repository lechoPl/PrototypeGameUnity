using UnityEngine;

namespace Assets.Scripts.Game.ValueObjects
{
    public class BoardSize
    {
        public float? MinX { get; private set; }

        public float? MaxX { get; private set; }

        public float? MinY { get; private set; }

        public float? MaxY { get; private set; }

        public float? Width
        {
            get
            {
                if(!MinX.HasValue || !MaxX.HasValue)
                {
                    return null;
                }

                return Mathf.Abs(MaxX.Value - MinX.Value);
            }
        }

        public float? Height
        {
            get
            {
                if (!MinY.HasValue || !MaxY.HasValue)
                {
                    return null;
                }

                return Mathf.Abs(MaxY.Value - MinY.Value);
            }
        }

        public float? MaxSize
        {
            get
            {
                var height = Height;
                var width = Width;

                float? result = height;

                if(!result.HasValue || (width.HasValue && result.Value < width.Value))
                {
                    result = width;
                }



                return result;
            }
        }

        public void Update(Field field)
        {
            float minX = field.transform.position.x - field.Bounds.Width / 2;
            float maxX = field.transform.position.x + field.Bounds.Width / 2;
            float minY = field.transform.position.y - field.Bounds.Height / 2;
            float maxY = field.transform.position.y + field.Bounds.Height / 2;

            MinX = !MinX.HasValue || MinX.Value > minX ? minX : MinX;
            MaxX = !MaxX.HasValue || MaxX.Value < maxX ? maxX : MaxX;
            MinY = !MinY.HasValue || MinY.Value > minY ? minY : MinY;
            MaxY = !MaxY.HasValue || MaxY.Value < maxY ? maxY : MaxY;
        }
    }
}
