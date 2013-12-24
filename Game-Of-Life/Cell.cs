using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Of_Life
{
    class Cell
    {
        private static readonly Dictionary<State, string> STATE_MATCH;

        private enum State { Alive, Emerging, Dying, Empty, Dead };

        private State state;

        static Cell()
        {
            STATE_MATCH = new Dictionary<State, string>();
            STATE_MATCH.Add(State.Alive, "#7700AA00");
            STATE_MATCH.Add(State.Emerging, "#7700CCCC");
            STATE_MATCH.Add(State.Empty, "FF111111");
            STATE_MATCH.Add(State.Dying, "#77FF0000");
            STATE_MATCH.Add(State.Dead, "#55FF0000");
        }

        public Cell()
        {
            this.state = State.Empty;
        }

        public string GetRender()
        {
            return STATE_MATCH[state];
        }

        public void SetAlive()
        {
            state = State.Alive;
        }

        public void SetEmerging()
        {
            state = State.Emerging;
        }

        public void SetDying()
        {
            state = State.Dying;
        }

        public void SetEmpty()
        {
            state = State.Empty;
        }

        public void SetDead()
        {
            state = State.Dead;
        }

        public bool IsDying()
        {
            return state == State.Dying;
        }

        public bool IsEmerging()
        {
            return state == State.Emerging;
        }

        public bool IsAlive()
        {
            return state == State.Alive;
        }

        public bool isDead()
        {
            return state == State.Dead;
        }

        public bool isEmpty()
        {
            return state == State.Empty;
        }

        public bool IsConsideredLikeDead()
        {
            return state == State.Empty || state == State.Dead || state == State.Dying;
        }

        public bool IsConsideredLikeAlive()
        {
            return state == State.Alive || state == State.Emerging;
        }
    }
}
