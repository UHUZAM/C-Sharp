using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            //Default values
            string sat_type = "";
            string station = "";
            string reference= "";
            string originator = "";
            string destinataire = "";
            string segment_name = "";
            string production_type = "";
            string spectral_type = "";
            string num_line = "";
            string num_col = "";
            string aoi_file= "";
            string mnt_or_dtm_mode= "AUTO";
            string pixel_coding= "16";
            string image_format= "JPEG2000";
            string compression_type = "";
            string ortho_mode= "AUTO";
            string projection_code = "";
            string parameters = "";
            string shipping_compression = "NONE";
            string pan_restoration = "Y";
            string restoration_scenario = "F_CONVOLUTION";
            string dtm_registration_scenario = "NONE";
            string use_mask = "Y";
            bool controle_cqi=false;
            char delimeterChar1 = ' ';
            char delimeterChar2 = '_';
            char delimeterChar3 = ' ';
            char delimeterChar4 = ',';
            string aoi;

            double average_long=0.0;
            double average_lat=0.0;
            DateTime current_time;
            byte[] m_ps = new byte[9];//mandatory parameters
            Array.Clear(m_ps, 0, 8);
            parameters = System.Console.ReadLine();
            string[] parameterArray1 = parameters.Split(delimeterChar1);
            string[] segment_array;

            for(int i=0;i<parameterArray1.Length;i+=2)
            {
                
                if (parameterArray1[i] == "-s")
                {
                    if (parameterArray1[i+1].Length == 2 && parameterArray1[i+1].All(char.IsDigit))
                    {
                        station = parameterArray1[i+1];
                    }
                    else
                        goto Error;
                    m_ps[0]++;
                }
                else if (parameterArray1[i] == "-r")
                {
                    if (parameterArray1[i+1].Length <= 16)
                    {
                        reference = parameterArray1[i+1];
                    }
                    else
                        goto Error;
                    m_ps[1]++;

                }
                else if (parameterArray1[i] == "-o")
                {
                    originator = parameterArray1[i + 1];
                    m_ps[2]++;

                }
                else if (parameterArray1[i] == "-d")
                {
                    destinataire = parameterArray1[i + 1];
                    m_ps[3]++;

                }
                else if (parameterArray1[i] == "-i")
                {
                    segment_name = parameterArray1[i+1];
                    m_ps[4]++;

                    
                }
                else if (parameterArray1[i] == "-p")
                {
                    if(parameterArray1[i+1]=="RS" || parameterArray1[i+1]=="OR" || parameterArray1[i+1] =="PS" )
                    {
                        production_type = parameterArray1[i+1];
                    }
                    else
                        goto Error;
                    m_ps[5]++;

                }
                else if (parameterArray1[i] == "-k")
                {
                    if (parameterArray1[i + 1] == "BUNDLE" || parameterArray1[i + 1] == "PAN" || parameterArray1[i + 1] == "XS" || parameterArray1[i + 1] == "BL-4B" || parameterArray1[i + 1] == "XS-4B" || parameterArray1[i + 1] == "PAN" || parameterArray1[i + 1] == "PMS" || parameterArray1[i + 1] == "PMS-N")
                    {
                        spectral_type = parameterArray1[i + 1];
                    }
                    else
                        goto Error;
                    m_ps[6]++;

                }
                else if (parameterArray1[i] == "-l")
                {
                    num_line = parameterArray1[i + 1];
                    m_ps[7]++;

                }
                else if (parameterArray1[i] == "-z")
                {
                    num_col = parameterArray1[i + 1];
                    m_ps[8]++;

                }
                else if (parameterArray1[i] == "-a")
                {
                    aoi_file = parameterArray1[i + 1]; 
                   
                }
                else if (parameterArray1[i] == "-c")
                {
                    if(parameterArray1[i + 1] == "16" || parameterArray1[i+1] == "8")
                    {
                        pixel_coding = parameterArray1[i + 1];
                    }
                    else 
                        goto Error;
                }
                else if (parameterArray1[i] == "-f")
                {
                    if(parameterArray1[i + 1] == "JPEG2000" || parameterArray1[i+1] == "GEOTIFF")
                    {
                        image_format = parameterArray1[i + 1];
                    }
                    else
                        goto Error;
                }
                else if (parameterArray1[i] == "-j")
                {
                    if (parameterArray1[i + 1] == "REVERSIBLE" || parameterArray1[i + 1] == "NOMINAL" || parameterArray1[i + 1] == "LOSSLESS" || parameterArray1[i + 1] == "LOSSY")
                    {
                        compression_type = parameterArray1[i + 1];
                    }
                    else
                        goto Error;
                }
                else if (parameterArray1[i] == "-m")
                {
                    if (parameterArray1[i + 1] == "GLOBE" || parameterArray1[i + 1] == "SRTM" || parameterArray1[i + 1] == "SYSTEM" || parameterArray1[i + 1] == "AUTO" || parameterArray1[i + 1] == "SRTM" || parameterArray1[i + 1] == "GTOPO30" || parameterArray1[i + 1] == "USER_DEFINED")
                    {
                        mnt_or_dtm_mode = parameterArray1[i + 1];
                    }
                    else
                        goto Error;
                }
                else if (parameterArray1[i] == "-t")
                {
                    if (parameterArray1[i + 1] == "AUTO" || parameterArray1[i + 1] == "SYSTEM" || parameterArray1[i + 1] == "ACCURATE")
                    {
                        ortho_mode = parameterArray1[i + 1];
                    }
                    else
                        goto Error;
                }
                else if (parameterArray1[i] == "-q")
                {
                    projection_code = parameterArray1[i + 1];
                }
                else if (parameterArray1[i] == "-y")
                {
                    controle_cqi = true;
                    i--;
                }
                else if (parameterArray1[i] == "-u")
                {
                    sat_type = parameterArray1[i + 1];
                }

            }
            if(sat_type == "")
            {
                Console.WriteLine("SPOT/PHR");
                sat_type = Console.ReadLine();
                if (sat_type != "SPOT" && sat_type != "PHR")
                {
                    Console.WriteLine("Invalid satallite type");
                    return;
                }
            }

            current_time = DateTime.UtcNow;
            var file_kml = "";
            if(aoi_file !="")
            {
                file_kml = System.IO.File.ReadAllText(@"\XMLgen\" + aoi_file);
            }
            var file_contents ="";

            segment_array = segment_name.Split(delimeterChar2);
            if (sat_type == "SPOT" && m_ps[0] == 1 && m_ps[1] == 1 && m_ps[2] == 1 && m_ps[3] == 1 && m_ps[4] == 1 && m_ps[5] == 1 && m_ps[6] == 1 && m_ps[7] == 1 && m_ps[8] == 1)
            {
                file_contents = System.IO.File.ReadAllText(@"\XMLgen\OE_DRS_" + production_type + ".template");
                if (segment_array[1] == "SPOT7" || segment_array[1] == "SPOT6")
                {
                    file_contents = file_contents.Replace("#reference#", reference);
                    file_contents = file_contents.Replace("#origine#", originator);
                    file_contents = file_contents.Replace("#destinataire#", destinataire);
                    segment_array = segment_name.Split(delimeterChar2);
                    if (segment_array[1] == "SPOT7")
                        file_contents = file_contents.Replace("#mission_id#", "7");
                    else
                        file_contents = file_contents.Replace("#mission_id#", "6");
                    file_contents = file_contents.Replace("#data_input#", segment_name);
                }
                if(aoi_file !="")
                {
                string coordinates = file_kml.Substring((file_kml.IndexOf("<coordinates>") + "<coordinates>".Length), (file_kml.IndexOf("</coordinates>") - file_kml.IndexOf("<coordinates>") - "<coordinates>".Length));
                string[] coordinate_array = coordinates.Split(delimeterChar3);
                aoi = "<Processed_ROI_Description>\n				<PROCESSED_ROI_TYPE>POLYGON_GEO</PROCESSED_ROI_TYPE>\n				<Polygon_Characteristics>\n";


                for (int i = 0; i < coordinate_array.Length - 1; i++)
                {
                    string[] one_coordinate_array = coordinate_array[i].Split(delimeterChar4);
                    aoi = aoi + "					<Geo_Point>\n						<LATITUDE>" + one_coordinate_array[1] + "</LATITUDE>\n						<LONGITUDE>" + one_coordinate_array[0] + "</LONGITUDE>\n					</Geo_Point>\n";
                }
                aoi = aoi + "\t\t\t\t</Polygon_Characteristics>\n			</Processed_ROI_Description>";
                file_contents = file_contents.Replace("<!--AOI-->", aoi);
                for (int i = 0; i < coordinate_array.Length - 2; i++)
                {
                    string[] one_coordinate_array = coordinate_array[i].Split(delimeterChar4);
                    Console.WriteLine("sadadsa  " + one_coordinate_array[0]);
                    one_coordinate_array[0] = one_coordinate_array[0].Replace(".", ",");
                    one_coordinate_array[1] = one_coordinate_array[0].Replace(".", ",");

                    Console.WriteLine("sadadsaww  " + one_coordinate_array[0]);
                    average_long += double.Parse(one_coordinate_array[0]);
                    average_lat += double.Parse(one_coordinate_array[1]);
                    Console.WriteLine("sadadsaww  " + average_long);

                }
                average_long = average_long / (coordinate_array.Length - 2);
                average_lat = average_lat / (coordinate_array.Length - 2);
                average_long = (int)((average_long + 180) / 6) + 1;
                if (average_lat < 0)
                    file_contents = file_contents.Replace("#projection_code#", "325" + average_long);
                else
                    file_contents = file_contents.Replace("#projection_code#", "326" + average_long);
                }

                if (spectral_type == "BL-4B")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PAN</SPECTRAL_PROCESSING>\n\t\t\t<SPECTRAL_PROCESSING>XS</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "XS-4B")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>XS</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "PAN")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PAN</SPECTRAL_PROCESSING>");
                }
                else if (spectral_type == "PMS")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PANSharpened</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "PMS-N")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PANSharpened</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                if(production_type == "OR")
                {
                    if (spectral_type == "XS" || spectral_type == "XS-4B")
                    file_contents = file_contents.Replace("#resampling_step#", "6.0");
                    else
                    file_contents = file_contents.Replace("#resampling_step#","1.5");
                    if(projection_code != "")
                    {
                        file_contents = file_contents.Replace("#projection_code#", projection_code);
                    }
                    file_contents = file_contents.Replace("#ortho_mode#", ortho_mode);
                }
                file_contents = file_contents.Replace("#mnt_mode#", mnt_or_dtm_mode);
                file_contents = file_contents.Replace("#pixel_coding#", pixel_coding);
                file_contents = file_contents.Replace("#image_format#", image_format);
                if(compression_type == "")
                {
                    compression_type = "LOSSLESS";
                }
                if(image_format == "JPEG2000")
                {
                    file_contents = file_contents.Replace("#compression_type#", "JPEG2000_" + compression_type);
                }
                else if (image_format == "GEOTIFF")
                {
                    file_contents = file_contents.Replace("<Workshop_Parameter>\n						<PARAMETER_NAME>JPEG2000_COMPRESSION</PARAMETER_NAME>\n						<PARAMETER_VALUE>#compression_type#</PARAMETER_VALUE>\n					</Workshop_Parameter>\n","");
                }
                file_contents = file_contents.Replace("#size_col#", num_col);
                file_contents = file_contents.Replace("#size_lig#", num_line);
                if(pixel_coding == "8")
                    file_contents = file_contents.Replace("<!--dynamic_adaptation-->", "<Digital_Dynamic_Adaptation>\n<ADAPTATION_TYPE>MANUAL</ADAPTATION_TYPE>\n<Adaptation_Thresholds>\n<SPECTRAL_BAND>B0</SPECTRAL_BAND>\n<LOWER_THRESHOLD_LEVEL>0.00001</LOWER_THRESHOLD_LEVEL>\n<UPPER_THRESHOLD_LEVEL>0.99999</UPPER_THRESHOLD_LEVEL>\n</Adaptation_Thresholds>\n<Adaptation_Thresholds>\n<SPECTRAL_BAND>B1</SPECTRAL_BAND>\n<LOWER_THRESHOLD_LEVEL>0.00001</LOWER_THRESHOLD_LEVEL>\n<UPPER_THRESHOLD_LEVEL>0.99999</UPPER_THRESHOLD_LEVEL>\n</Adaptation_Thresholds>\n<Adaptation_Thresholds>\n<SPECTRAL_BAND>B2</SPECTRAL_BAND>\n<LOWER_THRESHOLD_LEVEL>0.00001</LOWER_THRESHOLD_LEVEL>\n<UPPER_THRESHOLD_LEVEL>0.99999</UPPER_THRESHOLD_LEVEL>\n</Adaptation_Thresholds>\n<Adaptation_Thresholds>\n<SPECTRAL_BAND>B3</SPECTRAL_BAND>\n<LOWER_THRESHOLD_LEVEL>0.00001</LOWER_THRESHOLD_LEVEL>\n<UPPER_THRESHOLD_LEVEL>0.99999</UPPER_THRESHOLD_LEVEL>\n</Adaptation_Thresholds>\n<Adaptation_Thresholds>\n<SPECTRAL_BAND>PAN</SPECTRAL_BAND>\n<LOWER_THRESHOLD_LEVEL>0.00001</LOWER_THRESHOLD_LEVEL>\n<UPPER_THRESHOLD_LEVEL>0.99999</UPPER_THRESHOLD_LEVEL>\n</Adaptation_Thresholds>\n</Digital_Dynamic_Adaptation>");
                if (controle_cqi == true)
                {
                    file_contents = file_contents.Replace("#control_cqi#", "FULL");
                }
                else
                {
                    file_contents = file_contents.Replace("#control_cqi#", "NONE");
                }
            }
            else if(sat_type == "PHR"  && m_ps[1] == 1 && m_ps[2] == 1 && m_ps[3] == 1 && m_ps[4] == 1 && m_ps[5] == 1 && m_ps[6] == 1  )
            {
                if(spectral_type =="BUNDLE")
                    file_contents = System.IO.File.ReadAllText(@"\XMLgen\PHR_OE_DRS.bundle.template");
                else
                    file_contents = System.IO.File.ReadAllText(@"\XMLgen\PHR_OE_DRS.template");
                if (segment_array[1] == "PHR1A" || segment_array[1] == "PHR1B")
                {
                    file_contents = file_contents.Replace("#reference#", reference);
                    file_contents = file_contents.Replace("#origine#", originator);
                    file_contents = file_contents.Replace("#destinataire#", destinataire);
                    segment_array = segment_name.Split(delimeterChar2);
                    if (segment_array[1] == "PHR1A")
                        file_contents = file_contents.Replace("#mission_id#", "1A");
                    else
                        file_contents = file_contents.Replace("#mission_id#", "1B");
                    file_contents = file_contents.Replace("#data_input#", segment_name);
                }
                file_contents = file_contents.Replace("#shipping_compression#", shipping_compression);
                if (production_type == "PS")
                {
                    file_contents = file_contents.Replace("#product_type#", "Perfect-Sensor");
                    file_contents = file_contents.Replace("#type_p#", "PS");
                }
                else
                {
                    file_contents = file_contents.Replace("#product_type#", "Ortho-Rectified");
                    file_contents = file_contents.Replace("#type_p#", "ORTHO");
                }
                if (aoi_file != "")
                {
                    string coordinates = file_kml.Substring((file_kml.IndexOf("<coordinates>") + "<coordinates>".Length), (file_kml.IndexOf("</coordinates>") - file_kml.IndexOf("<coordinates>") - "<coordinates>".Length));
                    string[] coordinate_array = coordinates.Split(delimeterChar3);

                    aoi = "<Processed_ROI_Description>\n				<PROCESSED_ROI_TYPE>POLYGON_GEO</PROCESSED_ROI_TYPE>\n				<Polygon_Characteristics>\n";
                    for (int i = 0; i < coordinate_array.Length - 1; i++)
                    {
                        string[] one_coordinate_array = coordinate_array[i].Split(delimeterChar4);
                        aoi = aoi + "					<Geo_Point>\n						<LATITUDE>" + one_coordinate_array[1] + "</LATITUDE>\n						<LONGITUDE>" + one_coordinate_array[0] + "</LONGITUDE>\n					</Geo_Point>\n";
                    }
                    aoi = aoi + "\t\t\t\t</Polygon_Characteristics>\n			</Processed_ROI_Description>";
                    file_contents = file_contents.Replace("<!--AOI-->", aoi);
                    for (int i = 0; i < coordinate_array.Length - 2; i++)
                    {
                        string[] one_coordinate_array = coordinate_array[i].Split(delimeterChar4);
                        Console.WriteLine("sadadsa  " + one_coordinate_array[0]);
                        //string temp = Regex.Match(one_coordinate_array[0], @"\w[0-9]*[.]{1}[0-9]{5}\w").Value;
                        one_coordinate_array[0] = one_coordinate_array[0].Replace(".", ",");
                        one_coordinate_array[1] = one_coordinate_array[0].Replace(".", ",");

                        Console.WriteLine("sadadsaww  " + one_coordinate_array[0]);

                        //Decimal temp = Convert.ToDecimal(one_coordinate_array[0]);
                        //average_long += Decimal.ToDouble(temp);
                        average_long += double.Parse(one_coordinate_array[0]);
                        average_lat += double.Parse(one_coordinate_array[1]);
                        Console.WriteLine("sadadsaww  " + average_long);

                    }
                    Console.WriteLine("sadadsa" + average_long);
                    average_long = average_long / (coordinate_array.Length - 2);
                    average_lat = average_lat / (coordinate_array.Length - 2);
                    Console.WriteLine("sadadsa" + average_long);
                    average_long = (int)((average_long + 180) / 6) + 1;
                    Console.WriteLine("sadadsa" + average_long);
                    file_contents = file_contents.Replace("<!--dynamic_adaptation-->", "<Digital_Dynamic_Adaptation>\n\t\t\t\t\t<ADAPTATION_TYPE>MANUAL</ADAPTATION_TYPE>\n\t\t\t\t\t<Adaptation_Thresholds>\n\t\t\t\t\t\t<SPECTRAL_BAND>B0</SPECTRAL_BAND>\n\t\t\t\t\t\t<LOWER_THRESHOLD_LEVEL>0.001</LOWER_THRESHOLD_LEVEL>\n\t\t\t\t\t\t<UPPER_THRESHOLD_LEVEL>0.999</UPPER_THRESHOLD_LEVEL>\n\t\t\t\t\t</Adaptation_Thresholds>\n\t\t\t\t\t<Adaptation_Thresholds>\n\t\t\t\t\t\t<SPECTRAL_BAND>B1</SPECTRAL_BAND>\n\t\t\t\t\t\t<LOWER_THRESHOLD_LEVEL>0.001</LOWER_THRESHOLD_LEVEL>\n\t\t\t\t\t\t<UPPER_THRESHOLD_LEVEL>0.999</UPPER_THRESHOLD_LEVEL>\n\t\t\t\t\t</Adaptation_Thresholds>\n\t\t\t\t\t<Adaptation_Thresholds>\n\t\t\t\t\t\t<SPECTRAL_BAND>B2</SPECTRAL_BAND>\n\t\t\t\t\t\t<LOWER_THRESHOLD_LEVEL>0.001</LOWER_THRESHOLD_LEVEL>\n\t\t\t\t\t\t<UPPER_THRESHOLD_LEVEL>0.999</UPPER_THRESHOLD_LEVEL>\n\t\t\t\t\t</Adaptation_Thresholds>\n\t\t\t\t\t<Adaptation_Thresholds>\n\t\t\t\t\t\t<SPECTRAL_BAND>B3</SPECTRAL_BAND>\n\t\t\t\t\t\t<LOWER_THRESHOLD_LEVEL>0.001</LOWER_THRESHOLD_LEVEL>\n\t\t\t\t\t\t<UPPER_THRESHOLD_LEVEL>0.999</UPPER_THRESHOLD_LEVEL>\n\t\t\t\t\t</Adaptation_Thresholds>\n\t\t\t\t\t<Adaptation_Thresholds>\n\t\t\t\t\t\t<SPECTRAL_BAND>PAN</SPECTRAL_BAND>\n\t\t\t\t\t\t<LOWER_THRESHOLD_LEVEL>0.001</LOWER_THRESHOLD_LEVEL>\n\t\t\t\t\t\t<UPPER_THRESHOLD_LEVEL>0.999</UPPER_THRESHOLD_LEVEL>\n\t\t\t\t\t</Adaptation_Thresholds>\n\t\t\t\t</Digital_Dynamic_Adaptation>");

                }
                if (average_lat < 0)
                    file_contents = file_contents.Replace("#projection_code#", "325" + average_long);
                else
                    file_contents = file_contents.Replace("#projection_code#", "324" + average_long);
                if (spectral_type == "XS")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>XS</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "PAN")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PAN</SPECTRAL_PROCESSING>");
                }
                else if (spectral_type == "PMS")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PANSharpened</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "PMS-N")
                {
                    file_contents = file_contents.Replace("<!--spectral_processing-->", "<SPECTRAL_PROCESSING>PANSharpened</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                }
                else if (spectral_type == "BUNDLE" )
                {
                    file_contents = file_contents.Replace("<!--spectral_processing_xs-->", "<SPECTRAL_PROCESSING>XS</SPECTRAL_PROCESSING>\n\t\t\t<MULTI_SPECTRAL_PRODUCT_BANDS>\n\t\t\t\t<BAND>B0</BAND>\n\t\t\t\t<BAND>B1</BAND>\n\t\t\t\t<BAND>B2</BAND>\n\t\t\t\t<BAND>B3</BAND>\n\t\t\t</MULTI_SPECTRAL_PRODUCT_BANDS>");
                    file_contents = file_contents.Replace("<!--spectral_processing_pan-->", "<SPECTRAL_PROCESSING>PAN</SPECTRAL_PROCESSING>");
                }
                if(production_type == "OR")
                {
                    if (spectral_type == "XS-4B" || spectral_type == "BL-4B" || spectral_type == "XS")
                    file_contents = file_contents.Replace("#resampling_step#", "6.0");
                    else
                    file_contents = file_contents.Replace("#resampling_step#","1.5");
                    if(projection_code != "")
                    {
                        file_contents = file_contents.Replace("#projection_code#", projection_code);
                    }
           

                }
                file_contents = file_contents.Replace("#dtm_mode#", mnt_or_dtm_mode);
                file_contents = file_contents.Replace("#pixel_coding#", pixel_coding);
                file_contents = file_contents.Replace("#image_format#", image_format);
                if(compression_type=="")
                    compression_type = "NOMINAL";
                file_contents = file_contents.Replace("#image_compression#", compression_type);
                file_contents = file_contents.Replace("#attitude_to_use#", ortho_mode);
                file_contents = file_contents.Replace("#pan_restoration#", pan_restoration);
                file_contents = file_contents.Replace("#restoration_scenario#", restoration_scenario);
                file_contents = file_contents.Replace("#dtm_registration_scenario#", dtm_registration_scenario);
                file_contents = file_contents.Replace("#use_mask#", use_mask);
                file_contents = file_contents.Replace("#size_col#", num_col);
                file_contents = file_contents.Replace("#size_lig#", num_line);
                if (controle_cqi == true)
                {
                    file_contents = file_contents.Replace("#control_cqi#", "FULL");
                }
                else
                {
                    file_contents = file_contents.Replace("#control_cqi#", "NONE");
                }

            }
            else
            {
                Console.WriteLine("Eksik giris yapildi!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(file_contents);
            Console.WriteLine(parameters);
            if (sat_type == "SPOT")
            {
                string filename = originator + "_ORDER_" + station + "_" + reference + "-" + current_time.Year + current_time.Month.ToString("00") + current_time.Day.ToString("00") + current_time.Hour + current_time.Minute.ToString("00") + current_time.Second.ToString("00") + current_time.Millisecond.ToString("000") + ".XML";
                System.IO.File.WriteAllText(@"/XMLgen/" + filename, file_contents);
            }
            else if (sat_type == "PHR")
            {
                string filename = originator + "_ORDER_TT_" + reference + "-" + current_time.Year + current_time.Month.ToString("00") + current_time.Day.ToString("00") + current_time.Hour + current_time.Minute.ToString("00") + current_time.Second.ToString("00") + current_time.Millisecond.ToString("000") + ".XML";
                System.IO.File.WriteAllText(@"/XMLgen/" + filename, file_contents);
            }
            

            
            System.Console.Read();
            Error:
            {
                Console.WriteLine("Yanlis giris");
            }
            System.Console.Read();
        }
    }
    

    


}
