﻿using AspectInjector.Broker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace AspectInjector.Tests.InterfaceInjection
{
	[TestClass]
	public class InterfaceInjectionTests
	{
		private ITestInterface _testClass;

		[TestInitialize]
		public void SetUp()
		{
			_testClass = (ITestInterface)new TestInterfaceProxyClass();
		}

		[TestMethod]
		public void Inject_AspectReference_Into_SecondConstructor()
		{
			Checker.Passed = false;
			var result = ((ITestInterface)new TestInterfaceProxyClass(1)).TestMethod("ok");
			Assert.AreEqual("ok", result);
			Assert.IsTrue(Checker.Passed);
		}

		[TestMethod]
		public void Inject_Method_Proxy()
		{
			Checker.Passed = false;
			var result = _testClass.TestMethod("ok");
			Assert.AreEqual("ok", result);
			Assert.IsTrue(Checker.Passed);
		}

		[TestMethod]
		public void Inject_Setter_Proxy()
		{
			Checker.Passed = false;
			_testClass.TestProperty = 4;
			Assert.IsTrue(Checker.Passed);
		}

		[TestMethod]
		public void Inject_Getter_Proxy()
		{
			Checker.Passed = false;
			var a = _testClass.TestProperty;
			Assert.IsTrue(Checker.Passed);
		}

		[TestMethod]
		public void Inject_EventAdd_Proxy()
		{
			Checker.Passed = false;
			_testClass.TestEvent += (s, e) => { };
			Assert.IsTrue(Checker.Passed);
		}

		[TestMethod]
		public void Inject_EventRemove_Proxy()
		{
			Checker.Passed = false;
			_testClass.TestEvent -= (s, e) => { };
			Assert.IsTrue(Checker.Passed);
		}
	}

	[Aspect(typeof(TestInterfaceAspect))]
	//[Aspect(typeof(INotifyTest))]
	internal class TestInterfaceProxyClass
	{
		public TestInterfaceProxyClass()
		{
		}

		public TestInterfaceProxyClass(int a)
		{
		}
	}

	internal interface ITestInterface
	{
		string TestMethod(string data);

		event EventHandler<EventArgs> TestEvent;

		int TestProperty { get; set; }
	}

	[AdviceInterfaceProxy(typeof(INotifyPropertyChanged))]
	internal class INotifyTest : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
	}

	[AdviceInterfaceProxy(typeof(ITestInterface))]
	internal class TestInterfaceAspect : ITestInterface
	{
		string ITestInterface.TestMethod(string data)
		{
			Checker.Passed = true;
			return data;
		}

		public event System.EventHandler<System.EventArgs> TestEvent
		{
			add { Checker.Passed = true; }
			remove { Checker.Passed = true; }
		}

		public int TestProperty
		{
			get { Checker.Passed = true; return 0; }
			set { Checker.Passed = true; }
		}
	}
}