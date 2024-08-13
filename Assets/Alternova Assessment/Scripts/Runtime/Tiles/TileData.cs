
namespace Alternova.Runtime.Tiles
{
    [System.Serializable]
    public class TileData
    {
        public int R;
        public int C;
        public int number;

    }//Closes TileData struct

    [System.Serializable]
    public class TileDataWrapper
    {
        public TileData[] blocks;

    }//Closes TileDataWrapper struct

}//Closes Namespace declaration
