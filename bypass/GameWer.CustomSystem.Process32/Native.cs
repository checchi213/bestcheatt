using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GameWer.CustomSystem.Process32
{
	internal class Native
	{
		public struct ProcessEntry32
		{
			public uint dwSize;

			public uint cntUsage;

			public uint th32ProcessID;

			public IntPtr th32DefaultHeapID;

			public uint th32ModuleID;

			public uint cntThreads;

			public uint th32ParentProcessID;

			public int pcPriClassBase;

			public uint dwFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szExeFile;
		}

		private const string KERNEL32 = "kernel32.dll";

		private const string PSAPI = "psapi.dll";

		public static uint TH32CS_SNAPHEAPLIST = 1u;

		public static uint TH32CS_SNAPPROCESS = 2u;

		public static uint TH32CS_SNAPTHREAD = 4u;

		public static uint TH32CS_SNAPMODULE = 8u;

		public static uint TH32CS_SNAPMODULE32 = 16u;

		public static uint TH32CS_SNAPALL = 15u;

		public static uint TH32CS_INHERIT = 2147483648u;

		public static uint PROCESS_ALL_ACCESS = 2035711u;

		public static uint PROCESS_TERMINATE = 1u;

		public static uint PROCESS_CREATE_THREAD = 2u;

		public static uint PROCESS_VM_OPERATION = 8u;

		public static uint PROCESS_VM_READ = 16u;

		public static uint PROCESS_VM_WRITE = 32u;

		public static uint PROCESS_DUP_HANDLE = 64u;

		public static uint PROCESS_CREATE_PROCESS = 128u;

		public static uint PROCESS_SET_QUOTA = 256u;

		public static uint PROCESS_SET_INFORMATION = 512u;

		public static uint PROCESS_QUERY_INFORMATION = 1024u;

		public static uint PROCESS_SUSPEND_RESUME = 2048u;

		public static uint PROCESS_QUERY_LIMITED_INFORMATION = 4096u;

		public static uint SYNCHRONIZE = 1048576u;

		public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Process32First(IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Process32Next(IntPtr hSnapshot, ref ProcessEntry32 lppe);

		[DllImport("kernel32.dll")]
		public static extern int CloseHandle(IntPtr handle);

		[DllImport("psapi.dll")]
		public static extern uint GetProcessImageFileName(IntPtr hProcess, [Out] StringBuilder lpImageFileName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

		[DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, [Out] [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpExeName, ref uint lpdwSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
	}
}
