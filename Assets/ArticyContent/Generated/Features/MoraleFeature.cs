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
    public class MoraleFeature : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private Int32 mMoraleValue;
        
        public Int32 MoraleValue
        {
            get
            {
                return mMoraleValue;
            }
            set
            {
                mMoraleValue = value;
            }
        }
        
        private void CloneProperties(object aClone)
        {
            Articy.ManiacManfred.Features.MoraleFeature newClone = ((Articy.ManiacManfred.Features.MoraleFeature)(aClone));
            newClone.MoraleValue = MoraleValue;
        }
        
        public object CloneObject()
        {
            Articy.ManiacManfred.Features.MoraleFeature clone = new Articy.ManiacManfred.Features.MoraleFeature();
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
            if ((aProperty == "MoraleValue"))
            {
                MoraleValue = System.Convert.ToInt32(aValue);
                return;
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "MoraleValue"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(MoraleValue);
            }
            return null;
        }
        #endregion
    }
}
