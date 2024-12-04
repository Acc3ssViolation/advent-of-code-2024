using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day03_2 : IAssignment
    {
        private class FixedStringMatcher
        {
            private readonly string _needle;
            private int _index;

            public FixedStringMatcher(string needle)
            {
                _needle = needle ?? throw new ArgumentNullException(nameof(needle));
            }

            public bool PushChar(char c)
            {
                if (_needle[_index] == c)
                {
                    _index++;
                    if (_index == _needle.Length)
                    {
                        _index = 0;
                        return true;
                    }
                }
                else
                {
                    _index = 0;
                }
                return false;
            }
        }

        private class NumberMatcher
        {
            private bool _hasAny;
            private int _number;

            public bool PushChar(char c, out int result)
            {
                if (char.IsAsciiDigit(c))
                {
                    _number *= 10;
                    _number += c - '0';
                    result = _number;
                    return false;
                }
                else if (_hasAny)
                {
                    result = _number;
                    _hasAny = false;
                    _number = 0;
                    return true;
                }
                else
                {
                    result = _number;
                    _number = 0;
                    return true;
                }
            }
        }

        private class MulParser
        {
            private enum State
            {
                IDLE,
                DIGIT_A,
                DIGIT_B,
            }

            private FixedStringMatcher _prefixMatcher = new FixedStringMatcher("mul(");
            private NumberMatcher _numberMatcher = new NumberMatcher();

            private State _state;
            private int _argumentA;
            private int _argumentB;

            public bool PushChar(char c)
            {
                switch (_state)
                {
                    case State.IDLE:
                        if (_prefixMatcher.PushChar(c))
                        {
                            _argumentA = 0;
                            _state++;
                        }
                        break;

                    case State.DIGIT_A:
                        if (_numberMatcher.PushChar(c, out var num))
                        {
                            if (c == ',')
                            {
                                _argumentA = num;
                                _state++;
                            }
                            else
                            {
                                _state = State.IDLE;
                            }
                        }
                        break;

                    case State.DIGIT_B:
                        if (_numberMatcher.PushChar(c, out var num2))
                        {
                            if (c == ')')
                            {
                                _argumentB = num2;
                                _state = State.IDLE;
                                return true;
                            }
                            else
                            {
                                _state = State.IDLE;
                            }
                        }
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

        public string TestFile => "test-day03-2.txt";

        public string Run(IReadOnlyList<string> input)
        {
            var doMatcher = new FixedStringMatcher("do()");
            var doNotMatcher = new FixedStringMatcher("don't()");
            var parser = new MulParser();
            var enabled = true;
            var sum = 0;
            foreach (var line in input)
            {
                foreach (var c in line)
                {
                    if (doMatcher.PushChar(c))
                    {
                        enabled = true;
                    }
                    if (doNotMatcher.PushChar(c))
                    {
                        enabled = false;
                    }
                    if (parser.PushChar(c))
                    {
                        if (enabled)
                            sum += parser.Evaluate();

                        parser.Clear();
                    }
                }
            }
            return sum.ToString();
        }
    }
}
