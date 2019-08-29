/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: GeneralCheng
 * Description: 长安渝北工厂焊装车间WIMS系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Domain._04_IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NFine.Application.SystemManage
{
    public class CarMaintenanceAPP
    {
        private ICarMaintenanceRepository service = new CarMaintenanceRepository();

        public List<CarMaintenanceEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<CarMaintenanceEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.car.Contains(keyword));
                expression = expression.Or(t => t.option_code.Contains(keyword));
            }
            expression = expression.And(t => t.id>0);
            return service.IQueryable(expression).OrderBy(t => t.id).ToList();
        }
        public CarMaintenanceEntity GetForm(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                return service.FindEntity(int.Parse(keyValue));
            }
            else
            {
                CarMaintenanceEntity cm = new CarMaintenanceEntity();
                return cm;
            }
        }
        public void DeleteForm(int keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void SubmitForm(CarMaintenanceEntity CarMaintenanceEntity, int keyValue)
        {
            if(keyValue > 0)
            {
                //CarMaintenanceEntity.Modify(keyValue.ToString());
                service.Update(CarMaintenanceEntity);
            }
            else
            {
                //CarMaintenanceEntity.Create();
                service.Insert(CarMaintenanceEntity);
            }

            //service.SubmitForm(CarMaintenanceEntity, keyValue);
        }
    }

    
}
