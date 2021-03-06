﻿using System;
using System.IO;
using NUnit.Framework;
using OpenFAST.Codec;
using OpenFAST.Template;
using OpenFAST.Template.Operators;
using OpenFAST.Template.Types;

namespace OpenFAST.PerfUnitTest
{
    [TestFixture]
    internal class PerfUnitTest
    {
        public static void Main()
        {
            var t = new PerfUnitTest();

            //while (true)
            {
                try
                {
                    t.SetUp();
                    t.TestPerf();
                }
                finally
                {
                    t.TearDown();
                    //Console.WriteLine("Press Enter");
                    //Console.ReadLine();
                }
            }
        }

        #region Setup/Teardown

        [SetUp]
        protected void SetUp()
        {
            _context = new Context();

            var zero = new IntegerValue(0);
            var emptyStr = new StringValue("");

            _context.RegisterTemplate(
                'Q',
                new MessageTemplate(
                    "Quote",
                    new[]
                        {
                            new Scalar("SYMBOL", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("TIME_STAMP", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("BID_EXCH_Q", FastType.String, Operator.Copy, emptyStr, false),
                            new Scalar("SEQ_NUM", FastType.U32, Operator.Increment, zero, false),
                            new Scalar("BID_Q", FastType.I32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("BID_SIZE_Q", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("DATE_STAMP", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("SCALE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ITEM_TYPE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("CONDITION", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ASK_EXCH_Q", FastType.String, Operator.Copy, emptyStr, false),
                            new Scalar("ASK_Q", FastType.I32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ASK_SIZE_Q", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                        }));

            _context.RegisterTemplate(
                'T',
                new MessageTemplate(
                    "Trade",
                    new[]
                        {
                            new Scalar("SYMBOL", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("TIME_STAMP", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("EXCH", FastType.String, Operator.Copy, emptyStr, false),
                            new Scalar("SEQ_NUM", FastType.U32, Operator.Increment, zero, false),
                            new Scalar("PRICE", FastType.I32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("VOLUME", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("DATE_STAMP", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("SCALE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ITEM_TYPE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("CONDITION", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                        }));

            _context.RegisterTemplate(
                'B',
                new MessageTemplate(
                    "Bid",
                    new[]
                        {
                            new Scalar("SYMBOL", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("TIME_STAMP", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("EXCH", FastType.String, Operator.Copy, emptyStr, false),
                            new Scalar("SEQ_NUM", FastType.U32, Operator.Increment, zero, false),
                            new Scalar("PRICE_B", FastType.I32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("VOLUME_B", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("DATE_STAMP", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("SCALE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ITEM_TYPE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("CONDITION", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                        }));
            _context.RegisterTemplate(
                'A',
                new MessageTemplate(
                    "Ask",
                    new[]
                        {
                            new Scalar("SYMBOL", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("TIME_STAMP", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("EXCH", FastType.String, Operator.Copy, emptyStr, false),
                            new Scalar("SEQ_NUM", FastType.U32, Operator.Increment, zero, false),
                            new Scalar("PRICE_A", FastType.I32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("VOLUME_A", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("DATE_STAMP", FastType.String, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("SCALE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("ITEM_TYPE", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                            new Scalar("CONDITION", FastType.U32, Operator.Copy, ScalarValue.Undefined, false),
                        }));
        }

        [TearDown]
        protected void TearDown()
        {
            _context = null;
        }

        #endregion

        private Context _context;

        private const string LongFile = @"c:\projects\OpenSource\OpenFastNet\PerfUnitTest\test2.fast";

        [Test]
        public void TestPerf()
        {
            using (FileStream stream = File.Open(LongFile, FileMode.Open, FileAccess.Read))
            {
                var decoder = new FastDecoder(_context,new BufferedStream(stream,65535));

                foreach (Message msg in decoder)
                {
                    var type = (char) msg.GetInt(0);

                    string symbol = msg.GetString(1);
                    int t = msg.GetInt(2);
                    string exch = msg.GetString(3);
                    var seqNum = (uint) msg.GetInt(4);
                    int v5 = msg.GetInt(5);
                    var v6 = (uint) msg.GetInt(6);
                    int dt = msg.GetInt(7);
                    int v8 = msg.GetInt(8);
                    var itemType = (uint) msg.GetInt(9);
                    int v10 = msg.GetInt(10);

                    if (type == 'Q')
                    {
                        string v11 = msg.GetString(11);
                        int v12 = msg.GetInt(12);
                        var v13 = (uint) msg.GetInt(13);
                    }
                }
            }
        }
    }
}