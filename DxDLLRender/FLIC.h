namespace FLIC
{

	//@FACE Licence v2 //C++ Only, /MT CompilerFlag
	__declspec(noinline) void ParseFile(char* key)
	{
		VMProtectBeginUltra("FLIC::ParseFile"); FILE* lic = NULL;
		fopen_s(&lic, VMProtectDecryptStringA("C:\\ProgramData\\FACE.serial"), VMProtectDecryptStringA("r"));
		fgets(key, 1000, lic); fclose(lic); VMProtectEnd();
	}

	__declspec(noinline) void CheckLicence(const char* key)
	{
		VMProtectBeginUltra("FLIC::CheckLicence");
		VMProtectSetSerialNumber(key);
		VMProtectEnd();
	}

	__declspec(noinline) void Start()
	{
		VMProtectBeginUltra("FLIC::Start");
		char key[1000]; ParseFile(key);
		CheckLicence(key); VMProtectEnd();
	}
}