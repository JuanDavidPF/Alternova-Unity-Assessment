
using Alternova.Runtime.Tiles;
using Payosky.Core.PlainEvent;

namespace Alternova.Runtime.Events
{
    public class CardFlippedEvent : PlainEvent
    {
        readonly Tile card;
        public Tile Card => card;

        public CardFlippedEvent(Tile card)
        {
            this.card = card;
        }

    }//Closes CoverBoard
}//Closes Namespace declaration