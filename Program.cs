using System;
using System.IO;
using System.Diagnostics;
namespace vmd60t30
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length>1||args==null) {
				Console.WriteLine("error args!");
				Process.GetCurrentProcess().Kill();
			}
			string file_name=args[0];
			string[] n_name_list=file_name.Split('.');
			string n_name=n_name_list[0]+".30.vmd";
			put_log(file_name);
			put_log("save>"+n_name);
			FileStream vmd=new FileStream(file_name,FileMode.Open);
			FileStream n_vmd=new FileStream(n_name,FileMode.Create);
			int js_off=50;
			int rel_count=0;
			BinaryReader vmd_read=new BinaryReader(vmd);
			BinaryWriter n_vmd_read=new BinaryWriter(n_vmd);
			byte[] ttt={0x00,0x00,0x00,0x00};
			n_vmd.Write(vmd_read.ReadBytes(50),0,50);
			n_vmd_read.Write(ttt);
			int frame_count=vmd_read.ReadInt32();
			for (int i = 0; i < frame_count; i++) {
				byte[] bone_name=vmd_read.ReadBytes(0xf);
				Int32 frame_key=vmd_read.ReadInt32();
				if ((frame_key%2) == 0) {
					byte[] frame_data=new byte[0x5c];
					frame_data=vmd_read.ReadBytes(0x5c);
					int rel_fram=frame_key/2;
					n_vmd_read.Write(bone_name);
					n_vmd_read.Write(rel_fram);
					n_vmd_read.Write(frame_data);
					rel_count+=1;
				}else{
					vmd.Seek(0x5c,SeekOrigin.Current);
				}
			}
			n_vmd_read.Write(ttt);
			n_vmd_read.Write(ttt);
			n_vmd_read.Write(ttt);
			n_vmd_read.Seek(js_off,SeekOrigin.Begin);
			n_vmd_read.Write(rel_count);
			n_vmd.Dispose();
			vmd.Dispose();
		}
		static void put_log(string e)
		{
			Console.WriteLine(e);
		}
	}
}
