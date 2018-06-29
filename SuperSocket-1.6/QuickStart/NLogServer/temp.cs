using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogServer
{
    class temp
    {
    }
}
/*
                 // SN+null+PWD+null+status+null
                 var iNullPos = headerInfo.IndexOf(nullDel);

                 if (iNullPos>0)//found first null pos. we can read SN
                 {                         
                     SN =headerInfo.Substring(0, iNullPos);
                     if(validateSN(ref SN))
                     {
                         isDecodedSN = true;                            
                     }
                     //var headerInfoLeftOf= headerInfo.Remove(0, iNullPos+1);//+1 to include nullDel
                     //headerInfo = headerInfoLeftOf;
                 }
                 else
                 {
                    break;
                 }
                 idx = iNullPos + 1;
                 //idx = 0;
                 var iNullPosPwd = headerInfo.Substring(idx).IndexOf(nullDel);

                 if (iNullPosPwd >= 0)//found second null pos. we can read pwd
                 {
                     var pwdLength = iNullPosPwd;
                     pwd = headerInfo.Substring(idx,pwdLength);
                     isDecodedPwd = true;                        
                   //   var headerInfoLeftOf = headerInfo.Remove(0, iNullPosPwd + 1);//+1 to include nullDel
                   //   headerInfo = headerInfoLeftOf;
                 }
                 else
                 {
                     break;
                 }
                 idx += iNullPosPwd +1;
                 //idx = 0;
                 // get status info

                 var iNullPosStatus = headerInfo.Substring(idx).IndexOf(nullDel);
                 if (iNullPosStatus >= 0)//found third null pos. we can read status
                 {
                     var statusLength = iNullPosStatus;
                     string statusString = headerInfo.Substring(idx, statusLength);
                     //split status in parts
                     var vparts = statusString.Split(new char[] { '|' });
                     statusParts = vparts;
                     isDecodedStatus = true;
                     status = statusString;                        

                     nlogStatus = new NLogStatus
                     {
                         Version = vparts[0],
                         PORNr = vparts[1],
                         WDNr = vparts[2],
                         PUTime = vparts[3],
                         RSSI = vparts[4],
                         BatV = vparts[5],
                         ACQ = vparts[6],
                         DF = vparts[7]
                     };


                     // var headerInfoLeftOf = headerInfo.Remove(0, iNullPosStatus + 1);//+1 to include nullDel
                     // headerInfo = headerInfoLeftOf;

                 }
                 else
                 {
                     break;
                 }
                 idx += iNullPosStatus+1;
                 // idx = 0;
                 // at this point we have read all headerInfo. change state and move to data collection section
                 comState = sessionstate.HEADER_DONE;

                 // check if we have more from the data section

                 //check if we have any data segment already
                 var lAfterNullPosStatus = headerInfo.Length - idx;

                 startDataIdx = headerInfo.IndexOf(startDataDel);
                 EndDataIdx = headerInfo.IndexOf(endDataDel);

                 if (EndDataIdx >= 0 && startDataIdx>=0)//full message in one shot
                 {
                     comState = sessionstate.dataEnded;
                     var datalength = EndDataIdx - startDataIdx;
                     var str = headerInfo.Substring(startDataIdx, datalength);
                     data += str;
                     isDecodedData = true;
                     stage = SessionStage.DATA;
                 }
                 else if (startDataIdx >= 0 && headerInfo.Length > startDataDel.Length)
                 {
                     comState = sessionstate.dataStarted;
                     var str = headerInfo.Substring(startDataIdx); // include first [ char
                     data += str;
                 }
                 else if (lAfterNullPosStatus>0)
                 {
                     var dataAfterHeader = headerInfo.Substring(idx, lAfterNullPosStatus);
                     data += dataAfterHeader;
                 }
                 */

//if (isDecodedSN)
//{
//    deviceRequest.Key = SN;
//    deviceRequest.DeviceSN = SN;
//}
//if (isDecodedPwd)
//{
//    deviceRequest.Pwd = pwd;
//}
//if (isDecodedStatus)
//{
//    deviceRequest.statusParts = statusParts;
//    deviceRequest.status = status;
//    deviceRequest.nLogStatus = nlogStatus;

//}
//if (isDecodedData)
//{
//    deviceRequest.data = data;                    
//}