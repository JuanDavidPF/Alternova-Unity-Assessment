
namespace Alternova.Runtime.Tiles
{
    public enum ETileState
    {
        LOCKED, //When the card is COVERED and CANNOT be flipped
        CLEARED, //When the card is REVEALED and CANNOT be flipped
        COVERED, //When the card is COVERED and CAN be flipped
        REVEALED, //When the card is REVEALED and CAN be flipped
        TRANSITIONING, //When the card is being animated

    }

}