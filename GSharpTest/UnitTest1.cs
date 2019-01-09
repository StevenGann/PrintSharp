using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GSharp;

namespace GSharpTest
{
    [TestClass]
    public class GSharpTests
    {
        [TestMethod]
        public void TestGcodeParseline()
        {
            string raw = "G1 F900 X49.728 Y37.435 E2.17868 ;Test";

            Command command = Gcode.ParseLine(raw);

            Assert.AreEqual(command.Text, raw);
            Assert.AreEqual(command.Code, "G1");
            Assert.AreEqual(command.Comment, "Test");
            Assert.AreNotEqual(command.Parameters, null);
            Assert.AreEqual(command.Parameters.Length, 4);
            Assert.AreEqual(command.Parameters[0], "F900");
            Assert.AreEqual(command.Parameters[1], "X49.728");
            Assert.AreEqual(command.Parameters[2], "Y37.435");
            Assert.AreEqual(command.Parameters[3], "E2.17868");
        }
    }
}