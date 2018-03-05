// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Articy.Unity.Interfaces;
using Articy.Unity;
using Articy.ManiacManfred;

namespace Articy.ManiacManfred.Features
{
    
    
    [Serializable()]
    public class DisplayConditionFeature : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private ArticyValueArticyScriptCondition mShowMeIf = new ArticyValueArticyScriptCondition();
        
        public ArticyScriptCondition ShowMeIf
        {
            get
            {
                return mShowMeIf.GetValue();
            }
            set
            {
                mShowMeIf.SetValue(value);
            }
        }
        
        private void CloneProperties(object aClone)
        {
            Articy.ManiacManfred.Features.DisplayConditionFeature newClone = ((Articy.ManiacManfred.Features.DisplayConditionFeature)(aClone));
            if ((ShowMeIf != null))
            {
                newClone.ShowMeIf = ((ArticyScriptCondition)(ShowMeIf.CloneObject()));
            }
        }
        
        public object CloneObject()
        {
            Articy.ManiacManfred.Features.DisplayConditionFeature clone = new Articy.ManiacManfred.Features.DisplayConditionFeature();
            CloneProperties(clone);
            return clone;
        }
        
        public virtual bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return false;
        }
        
        #region property provider interface
        public void setProp(string aProperty, object aValue)
        {
            if ((aProperty == "ShowMeIf"))
            {
                ShowMeIf = ((ArticyScriptCondition)(aValue));
                return;
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "ShowMeIf"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ShowMeIf);
            }
            return null;
        }
        #endregion
    }
}
