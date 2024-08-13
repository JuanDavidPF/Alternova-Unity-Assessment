
using Alternova.Runtime.Tiles;
using Payosky.Core.PlainEvent;

namespace Alternova.Runtime.Events
{
    public class CardClearedEvent : PlainEvent
    {
        readonly Tile card;
        public Tile Card => card;

        public CardClearedEvent(Tile card)
        {
            this.card = card;
        }

    }//Closes CardClearedEvent event
}//Closes Namespace declaration