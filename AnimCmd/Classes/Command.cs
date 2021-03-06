﻿using System;
using System.Collections.Generic;

namespace Sm4shCommand.Classes
{
    public class Command
    {
        public Command(CommandInfo info)
        {
            _commandInfo = info;
        }
        public Command() { }

        public CommandInfo _commandInfo;


        public List<object> parameters = new List<object>();

        public virtual int CalcSize() { return 0x04 + (_commandInfo.ParamSpecifiers.Count * 4); }

        public override string ToString()
        {
            string Param = "";
            for (int i = 0; i < parameters.Count; i++)
            {
                if (_commandInfo.ParamSyntax.Count > 0)
                    Param += String.Format("{0}=", _commandInfo.ParamSyntax[i]);

                if (parameters[i] is int | parameters[i] is bint)
                    Param += String.Format("0x{0:X}{1}", parameters[i], i + 1 != parameters.Count ? ", " : "");
                if (parameters[i] is float | parameters[i] is bfloat)
                    Param += String.Format("{0}{1}", parameters[i], i + 1 != parameters.Count ? ", " : "");
                if (parameters[i] is decimal)
                    Param += String.Format("{0}{1}", parameters[i], i + 1 != parameters.Count ? ", " : "");

            }
            return String.Format("{0}({1})", _commandInfo.Name, Param);

        }
        public virtual byte[] GetArray()
        {
            byte[] tmp = new byte[CalcSize()];
            Util.SetWord(ref tmp, _commandInfo.Identifier, 0, Runtime.WorkingEndian);
            for (int i = 0; i < _commandInfo.ParamSpecifiers.Count; i++)
            {
                if (_commandInfo.ParamSpecifiers[i] == 0)
                    Util.SetWord(ref tmp, Convert.ToInt32(parameters[i]), (i + 1) * 4, Runtime.WorkingEndian);
                else if (_commandInfo.ParamSpecifiers[i] == 1)
                {
                    double HEX = Convert.ToDouble(parameters[i]);
                    float flt = (float)HEX;
                    byte[] bytes = BitConverter.GetBytes(flt);
                    int dec = BitConverter.ToInt32(bytes, 0);
                    string HexVal = dec.ToString("X");

                    Util.SetWord(ref tmp, Int32.Parse(HexVal, System.Globalization.NumberStyles.HexNumber), (i + 1) * 4, Runtime.WorkingEndian);
                }
                else if (_commandInfo.ParamSpecifiers[i] == 2)
                    Util.SetWord(ref tmp, (long)Convert.ToDecimal(parameters[i]), (i + 1) * 4, Runtime.WorkingEndian);
            }
            return tmp;
        }
    }

    public class UnknownCommand : Command
    {
        public uint ident;

        public override int CalcSize() { return 0x04; }
        public override string ToString()
        {

            return String.Format("0x{0:X8}", ident);
        }
        public override byte[] GetArray()
        {
            byte[] _data = new byte[4];
            Util.SetWord(ref _data, ident, 0, Runtime.WorkingEndian);

            return _data;
        }
    }
}
