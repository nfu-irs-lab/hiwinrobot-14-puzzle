using System;

namespace ExclusiveProgram
{
    public class Config : MainForm.Config
    {
        public override string ArmIp => "192.168.100.126";

        public override bool ArmEnable => true;

        public override string GripperComPort => "COM10";

        public override bool GripperEnable => true;
    }
}
