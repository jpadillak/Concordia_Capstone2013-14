﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace Rover.Commands
{
    public class TiltCommand : ICommand
    {
        private int camIndex;
        private int tiltAngle;
        private string rawCommand;
        private MicrocontrollerSingleton microcontroller;

        public int CameraIndex { get { return camIndex; } }
        public int Angle { get { return tiltAngle; } }
        public string RawCommand { get { return rawCommand; } }

        public TiltCommand(string unparsedCommand)
        {
            if (unparsedCommand == null)
            {
                throw new ArgumentNullException("Null string received");
            }

            rawCommand = unparsedCommand;
            try
            {
                camIndex = ParseCamIndex(unparsedCommand);
                tiltAngle = ParseTiltAngle(unparsedCommand);
            }
            catch (ArgumentException)
            {
                throw;
            }
            microcontroller = MicrocontrollerSingleton.Instance;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void UnExecute()
        {
            throw new NotImplementedException("There is no unexecute for tilt commands");
        }

        private int ParseCamIndex(string unparsedCommand)
        {
            string rawCamIndex;
            int index;

            rawCamIndex = unparsedCommand.Substring(CommandMetadata.Tilt.NumberIdentifierIndex, CommandMetadata.Tilt.NumberIdentifierLength);

            try
            {
                index = Convert.ToInt32(rawCamIndex);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid camera index " + rawCamIndex + " received for command " + unparsedCommand);
            }

            return index;

        }

        private int ParseTiltAngle(string unparsedText)
        {
            string tiltAngleStr;
            int tiltAngleNum;

            int readLength = CommandMetadata.Tilt.AngleEndIndex - CommandMetadata.Tilt.AngleStartIndex + 1;

            tiltAngleStr = unparsedText.Substring(CommandMetadata.Tilt.AngleStartIndex, readLength);

            try
            {
                tiltAngleNum = Convert.ToInt32(tiltAngleStr);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Pan angle received in " + rawCommand);
            }

            if (tiltAngleNum > CommandMetadata.Tilt.MaxTiltAngle)
            {
                throw new ArgumentException("Pan angle received higher than expected maximum in " + rawCommand);
            }
            else if (tiltAngleNum < CommandMetadata.Tilt.MinTiltAngle)
            {
                throw new ArgumentException("Pan angle received lower than expected minimum in" + rawCommand);
            }

            return tiltAngleNum;
        }
    }
}