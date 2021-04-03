using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;

    public class StringExtensionsTest
    {
        [Test]
        public void FormatForToString()
        {
            MySimpleType simple = new MySimpleType()
            {
                number = 1,
                word = "test"
            };

            MyComplexType complex = new MyComplexType()
            {
                complexWord = "complex",
                simpleType = simple
            };

            MyComplexInheritedType inherited = new MyComplexInheritedType()
            {
                inheritedWord = "inherited",
                complexWord = "complex",
                simpleType = simple
            };

            MyNestdComplexType nested = new MyNestdComplexType()
            {
                nestedWord = "nested",
                complexType = complex
            };

            MyNestdComplexType nestedInherited = new MyNestdComplexType()
            {
                nestedWord = "nested",
                complexType = inherited
            };

            Assert.AreEqual("{ number = 1 | word = test }", simple.ToString());
            Assert.AreEqual("{ complexWord = complex | simpleType = { number = 1 | word = test } }", complex.ToString());
            Assert.AreEqual("{ complexWord = complex | simpleType = { number = 1 | word = test } | inheritedWord = inherited }", inherited.ToString());
            Assert.AreEqual("{ nestedWord = nested | complexType = { complexWord = complex | simpleType = { number = 1 | word = test } } }", nested.ToString());
            Assert.AreEqual("{ nestedWord = nested | complexType = { complexWord = complex | simpleType = { number = 1 | word = test } | inheritedWord = inherited } }", nestedInherited.ToString());
        }

        private class MySimpleType
        {
            public int number;
            public string word;

            public override string ToString()
            {
                string[] titles = new string[]
                {
                    "number",
                    "word"
                };

                object[] values = new object[]
                {
                    number,
                    word
                };

                return StringExtensions.FormatForToString(titles, values);
            }
        }

        private class MyComplexType
        {
            public string complexWord;
            public MySimpleType simpleType;

            public override string ToString()
            {
                string[] titles = new string[]
                {
                    "complexWord",
                    "simpleType"
                };

                object[] values = new object[]
                {
                    complexWord,
                    simpleType
                };

                return StringExtensions.FormatForToString(titles, values);
            }
        }

        private class MyComplexInheritedType : MyComplexType
        {
            public string inheritedWord;

            public override string ToString()
            {
                string[] titles = new string[]
                {
                    "inheritedWord"
                };

                object[] values = new object[]
                {
                    inheritedWord
                };

                return StringExtensions.FormatForToString(titles, values, base.ToString());
            }
        }

        private class MyNestdComplexType
        {
            public string nestedWord;
            public MyComplexType complexType;

            public override string ToString()
            {
                string[] titles = new string[]
                {
                    "nestedWord",
                    "complexType"
                };

                object[] values = new object[]
                {
                    nestedWord,
                    complexType
                };

                return StringExtensions.FormatForToString(titles, values);
            }
        }
    }
}
