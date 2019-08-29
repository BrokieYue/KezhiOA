/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Domain.Entity.SystemSecurity
*文件名： OperateLogEntity
*创建人： 劉嶺
*创建时间：2018/7/16 11:32:22
*描述
*=====================================================================
*修改标记
*修改时间：2018/7/16 11:32:22
*修改人：劉嶺
*描述：
************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.Entity.SystemSecurity
{
    public class OperateLogEntity : IEntity<OperateLogEntity>, ICreationAudited
    {
        /// <summary>
        /// 
        /// </summary>
        public string F_Id { get; set; }
        /// <summary>
        /// 日志类型GUID
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 插入\删除\修改数据行主键GUID
        /// </summary>
        public string Object_ID { get; set; }
        /// <summary>
        /// 插入\删除\修改单据号
        /// </summary>
        public string Object_No { get; set; }
        /// <summary>
        /// 日志类型编码
        /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 备注/描述
        /// </summary>
        public string F_Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string F_CreatorUserId { get; set; }
    }
}

