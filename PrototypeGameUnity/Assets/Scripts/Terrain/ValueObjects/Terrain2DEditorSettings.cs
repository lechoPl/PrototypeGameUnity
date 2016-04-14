using Assets.Scripts.Terrain.Enums;
using System;

namespace Assets.Scripts.Terrain.ValueObjects
{
    [Serializable]
    public class Terrain2DEditorSettings
    {
        public bool editModeEnabled = true;
        public EditMode mode;
        public float markSize = 0.5f;
    }
}
