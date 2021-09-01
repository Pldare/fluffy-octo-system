using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PmxLib;

namespace fluffy_octo_system
{
    class Program
    {
        public static Matrix RotationAxis(Vector3 axis, float angle)
        {
            if (axis.LengthSquared() != 1f)
            {
                axis.Normalize();
            }
            Matrix result = default(Matrix);
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            double num = (double)angle;
            float num2 = (float)Math.Cos(num);
            float num3 = (float)Math.Sin(num);
            double num4 = (double)x;
            double num5 = num4;
            float num6 = (float)(num5 * num5);
            double num7 = (double)y;
            double num8 = num7;
            float num9 = (float)(num8 * num8);
            double num10 = (double)z;
            double num11 = num10;
            float num12 = (float)(num11 * num11);
            float num13 = (float)((double)y * (double)x);
            float num14 = (float)((double)z * (double)x);
            float num15 = (float)((double)z * (double)y);
            result.M11 = (float)((1.0 - (double)num6) * (double)num2 + (double)num6);
            double num16 = (double)num13;
            double num17 = num16 - (double)num2 * num16;
            double num18 = (double)num3 * (double)z;
            result.M12 = (float)(num18 + num17);
            double num19 = (double)num14;
            double num20 = num19 - (double)num2 * num19;
            double num21 = (double)num3 * (double)y;
            result.M13 = (float)(num20 - num21);
            result.M14 = 0f;
            result.M21 = (float)(num17 - num18);
            result.M22 = (float)((1.0 - (double)num9) * (double)num2 + (double)num9);
            double num22 = (double)num15;
            double num23 = num22 - (double)num2 * num22;
            double num24 = (double)num3 * (double)x;
            result.M23 = (float)(num24 + num23);
            result.M24 = 0f;
            result.M31 = (float)(num21 + num20);
            result.M32 = (float)(num23 - num24);
            result.M33 = (float)((1.0 - (double)num12) * (double)num2 + (double)num12);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }
        public static Matrix RotationQuaternion(Quaternion rotation)
        {
            Matrix result = default(Matrix);
            float x = rotation.X;
            double num = (double)x;
            double num2 = num;
            float num3 = (float)(num2 * num2);
            float y = rotation.Y;
            double num4 = (double)y;
            double num5 = num4;
            float num6 = (float)(num5 * num5);
            float z = rotation.Z;
            double num7 = (double)z;
            double num8 = num7;
            float num9 = (float)(num8 * num8);
            float num10 = (float)((double)y * (double)x);
            float w = rotation.W;
            float num11 = (float)((double)w * (double)z);
            float num12 = (float)((double)z * (double)x);
            float num13 = (float)((double)w * (double)y);
            float num14 = (float)((double)z * (double)y);
            float num15 = (float)((double)w * (double)x);
            result.M11 = (float)(1.0 - ((double)num9 + (double)num6) * 2.0);
            result.M12 = (float)(((double)num11 + (double)num10) * 2.0);
            result.M13 = (float)(((double)num12 - (double)num13) * 2.0);
            result.M14 = 0f;
            result.M21 = (float)(((double)num10 - (double)num11) * 2.0);
            result.M22 = (float)(1.0 - ((double)num9 + (double)num3) * 2.0);
            result.M23 = (float)(((double)num15 + (double)num14) * 2.0);
            result.M24 = 0f;
            result.M31 = (float)(((double)num13 + (double)num12) * 2.0);
            result.M32 = (float)(((double)num14 - (double)num15) * 2.0);
            result.M33 = (float)(1.0 - ((double)num6 + (double)num3) * 2.0);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }
        public static Vector3 TransformCoordinate(Vector3 coordinate, Matrix transformation)
        {
            Vector4 vector = default(Vector4);
            vector.X = (float)((double)transformation.M21 * (double)coordinate.Y + (double)transformation.M11 * (double)coordinate.X + (double)transformation.M31 * (double)coordinate.Z + (double)transformation.M41);
            vector.Y = (float)((double)transformation.M22 * (double)coordinate.Y + (double)transformation.M12 * (double)coordinate.X + (double)transformation.M32 * (double)coordinate.Z + (double)transformation.M42);
            vector.Z = (float)((double)transformation.M23 * (double)coordinate.Y + (double)transformation.M13 * (double)coordinate.X + (double)transformation.M33 * (double)coordinate.Z + (double)transformation.M43);
            float num = (float)(1.0 / ((double)transformation.M24 * (double)coordinate.Y + (double)transformation.M14 * (double)coordinate.X + (double)transformation.M34 * (double)coordinate.Z + (double)transformation.M44));
            vector.W = num;
            Vector3 result=new Vector3();
            result.X = (float)((double)vector.X * (double)num);
            result.Y = (float)((double)vector.Y * (double)num);
            result.Z = (float)((double)vector.Z * (double)num);
            return result;
        }
        //顶点坐标 旋转中心 旋转角度
        static Vector3 rot_with_base(Vector3 v,Vector3 o,Quaternion r)
        {
            Vector3 orgpos = v;
            Vector3 parpos = o;
            Vector3 locpos = orgpos - parpos;
            //PmxBoneMorph parpbm = (PmxBoneMorph)morp.OffsetList[par_is_morp];
            //Quaternion parot = parpbm.Rotaion;
            Matrix mat;
            Matrix.RotationQuaternion(ref r, out mat);
            return TransformCoordinate(locpos, mat) + parpos - orgpos;
        }
        static void Main(string[] args)
        {
            string file_name = args[0];
            string ext_name = Path.GetExtension(file_name);
            string base_name = Path.GetFileNameWithoutExtension(file_name);
            if (File.Exists(file_name))
            {
                using (FileStream fs = new FileStream(file_name, FileMode.Open))
                {
                    using (BinaryReader bs = new BinaryReader(fs))
                    {
                        //pmxx a = new pmxx(bs);
                        //if (a.load_head())
                        //{
                        //    a.load_config();
                        //    a.load_vertex();
                        //}
                        Pmx a = new Pmx();
                        a.FromStream(fs, false);
                        //Pmx aa = new Pmx();
                        List<PmxMorph> aa = new List<PmxMorph> { };
                        //PmxMorph npm = new PmxMorph();


                        PmxLib.Quaternion q = new PmxLib.Quaternion(0.707f, 0.0f, 0.707f, 0.0f);
                        PmxLib.Matrix ma;
                        Matrix.RotationQuaternion(ref q,out ma);//RotationYawPitchRoll(90, 0, 0);//RotationQuaternion(q);
                        Vector3 po = TransformCoordinate(new Vector3(1, 1, 1), ma);
                        Console.WriteLine(po.ToString());
                        foreach (PmxMorph morp in a.MorphList)
                        {
                            //break;
                            Console.WriteLine(morp.Name);
                            //Console.WriteLine(morp.IsBone);
                            //Console.WriteLine(morp.CID.ToString());
                            //Console.WriteLine(morp.UID.ToString());
                            
                            if (morp.IsBone)
                            {
                                string re_name = morp.Name;
                                string re_en_name = morp.NameE;
                                morp.Name = re_name + ".old";
                                morp.NameE = re_en_name + ".old";
                                PmxMorph npm = new PmxMorph();
                                npm.Name = re_name;
                                npm.NameE = re_en_name;
                                npm.Panel = morp.Panel;
                                //npm.Name = "1";
                                //a.MorphList.Add(npm);
                                //break;
                                //偏移计算
                                int[] bont_id_arry = new int[morp.OffsetList.Count];
                                for (int i = 0; i < morp.OffsetList.Count; i++)
                                {
                                    PmxBoneMorph pbb = (PmxBoneMorph)morp.OffsetList[i];
                                    bont_id_arry[i] = pbb.Index;
                                }
                                int tindex=0;
                                foreach (PmxBoneMorph pbm in morp.OffsetList)
                                {
                                    //Console.WriteLine(pbm.Index.ToString());
                                    //Console.WriteLine(pbm.Translation.ToString());
                                    PmxBone b = a.BoneList[pbm.Index];
                                    //b.Parent
                                    //Console.WriteLine(b.Name);
                                    //Console.WriteLine(pbm.Translation.ToString());
                                    //Console.WriteLine(pbm.Rotaion.ToString());
                                    //获取骨骼index
                                    int get_bone_index = pbm.Index;
                                    //获取骨骼偏移
                                    PmxLib.Vector3 offset = pbm.Translation;
                                    //获取骨骼旋转
                                    Quaternion rott = pbm.Rotaion;
                                    Vector3 rotv = new Vector3();
                                    //获取骨骼影响列表
                                    List<int> index = new List<int> { };
                                    List<PmxVertex> bone_arry = get_fix_ert_index(a, get_bone_index,ref index);
                                    //Console.WriteLine(bone_arry.Count);
                                    int js_index = 0;
                                    int par_is_morp = Array.IndexOf(bont_id_arry, a.BoneList[pbm.Index].Parent);
                                    
                                    foreach (PmxVertex pvv in bone_arry)
                                    {
                                        PmxVertexMorph pvm = new PmxVertexMorph();
                                        float info_weight = 1.0f;
                                        foreach (var item in pvv.Weight)
                                        {
                                            if (item.Bone==get_bone_index)
                                            {
                                                info_weight = item.Value;
                                            }
                                        }
                                        Vector3 pad = new Vector3();
                                        if (par_is_morp != -1)
                                        {
                                           // Console.WriteLine("true");
                                            int par_id = a.BoneList[pbm.Index].Parent;
                                            Vector3 parpos = a.BoneList[par_id].Position;
                                            Vector3 orgpos = pvv.Position;
                                            PmxBoneMorph parpbm = (PmxBoneMorph)morp.OffsetList[par_is_morp];
                                            Quaternion parot = parpbm.Rotaion;
                                            pad = rot_with_base(pvv.Position, a.BoneList[par_id].Position, parot);
                                            //Console.WriteLine(orgpos.ToString());
                                            //Console.WriteLine(parpos.ToString());
                                            //Console.WriteLine(locpos.ToString());
                                            
                                            //Console.WriteLine("父级ID:"+par_id.ToString());
                                            //Console.WriteLine("当前ID:" + pbm.Index.ToString());
                                            //Console.WriteLine("获得父级旋转:" + parot.ToString());
                                            //Console.WriteLine(pad.ToString());
                                            //Console.WriteLine(offset.ToString());
                                            //Console.WriteLine(index[js_index].ToString());
                                            //Vector3 vpos = a.VertexList[index[js_index]].Position;
                                            //Vector3 repos = vpos + offset + pad;
                                            //Console.WriteLine(vpos.ToString());
                                            //Console.WriteLine(repos.ToString());
                                            //Console.WriteLine(info_weight.ToString());
                                        }
                                        pvm.Index = index[js_index];
                                        js_index++;
                                        //Console.WriteLine(pvm.Index);
                                        //Console.WriteLine(offset.y);
                                        //计算旋转
                                        rotv = rot_with_base(pvv.Position, a.BoneList[pbm.Index].Position, rott);

                                        pvm.Offset.x = (offset.x + rotv.x + pad.x) * info_weight;
                                        pvm.Offset.y = (offset.y + rotv.y + pad.y) * info_weight;
                                        pvm.Offset.z = (offset.z + rotv.z + pad.z) * info_weight;
                                        if (pvm.Offset==new PmxLib.Vector3(0.0f,0.0f,0.0f))
                                        {
                                            continue;
                                        }
                                        npm.OffsetList.Add(pvm);
                                    }
                                    tindex++;
                                }
                                aa.Add(npm);
                            }
                        }
                        foreach (var item in aa)
                        {
                            a.MorphList.Add(item);
                        }
                        a.ToFile(base_name+"_vertmorp_"+ext_name);
                        
                    }
                }
            }
        }
        static List<PmxVertex> get_fix_ert_index(Pmx pvl,int bone_id,ref List<int> idex)
        {
            List<PmxVertex> barr = new List<PmxVertex> { };
            int id = 0;
            foreach (PmxVertex pvert in pvl.VertexList)
            {
                foreach (PmxVertex.BoneWeight bw in pvert.Weight)
                {
                    if (bw.Bone==bone_id)
                    {
                        barr.Add(pvert);
                        idex.Add(id);
                    }
                }
                id++;
            }
            return barr;
        }
    }
}
