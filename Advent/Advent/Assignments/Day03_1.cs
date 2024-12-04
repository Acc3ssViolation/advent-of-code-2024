using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent.Assignments
{
    internal class Day03_1 : IAssignment
    {
        private class Parser
        {
            private enum State
            {
                IDLE,
                LETTER_M,
                LETTER_U,
                LETTER_L,
                PAR_OPEN,
                DIGIT_A,
                COMMA,
                DIGIT_B,
                PAR_CLOSE,
            }

            private State _state;
            private int _argumentA;
            private int _argumentB;

            public bool PushChar(char c)
            {
                switch (_state)
                {
                    case State.IDLE:
                        if (c == 'm')
                            _state++;
                        else
                            _state = State.IDLE;
                        break;
                    case State.LETTER_M:
                        if (c == 'u')
                            _state++;
                        else
                            _state = State.IDLE;
                        break;
                    case State.LETTER_U:
                        if (c == 'l')
                            _state++;
                        else
                            _state = State.IDLE;
                        break;
                    case State.LETTER_L:
                        if (c == '(')
                            _state++;
                        else
                            _state = State.IDLE;
                        break;
                    case State.PAR_OPEN:
                        if (char.IsAsciiDigit(c))
                        {
                            _argumentA = c - '0';
                            _state++;
                        }
                        else
                            _state = State.IDLE;
                        break;
                    case State.DIGIT_A:
                        if (char.IsAsciiDigit(c))
                        {
                            _argumentA *= 10;
                            _argumentA += c - '0';
                        }
                        else if (c == ',')
                            _state++;
                        else
                            _state = State.IDLE;
                        break;

                    case State.COMMA:
                        if (char.IsAsciiDigit(c))
                        {
                            _argumentB = c - '0';
                            _state++;
                        }
                        else
                            _state = State.IDLE;
                        break;

                    case State.DIGIT_B:
                        if (char.IsAsciiDigit(c))
                        {
                            _argumentB *= 10;
                            _argumentB += c - '0';
                        }
                        else if (c == ')')
                        {
                            _state++;
                            return true;
                        }
                        else
                            _state = State.IDLE;
                        break;
                }

                return false;
            }

            public int Evaluate()
            {
                return _argumentA * _argumentB;
            }

            public void Clear()
            {
                _state = State.IDLE;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var parser = new Parser();
            var sum = 0;
            foreach (var line in input)
            {
                foreach (var c in line)
                {
                    if (parser.PushChar(c))
                    {
                        sum += parser.Evaluate();
                        parser.Clear();
                    }
                }
            }
            return sum.ToString();
        }
    }
}
