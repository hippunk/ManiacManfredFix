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
using Articy.Unity;
using Articy.Unity.Interfaces;

namespace Articy.ManiacManfred
{
    
    
    [Serializable()]
    public class IncomingConnection : IArticyBaseObject, IIncomingConnection, IObjectWithColor
    {
        
        [SerializeField()]
        private String mLabel;
        
        [SerializeField()]
        private Color mColor;
        
        [SerializeField()]
        private UInt64 mSourcePin;
        
        [SerializeField()]
        private ArticyValueArticyObject mSource = new ArticyValueArticyObject();
        
        public String Label
        {
            get
            {
                return mLabel;
            }
            set
            {
                mLabel = value;
            }
        }
        
        public Color Color
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
            }
        }
        
        public UInt64 SourcePin
        {
            get
            {
                return mSourcePin;
            }
            set
            {
                mSourcePin = value;
            }
        }
        
        public ArticyObject Source
        {
            get
            {
                return mSource.GetValue();
            }
            set
            {
                mSource.SetValue(value);
            }
        }
        
        private void CloneProperties(object aClone)
        {
            IncomingConnection newClone = ((IncomingConnection)(aClone));
            newClone.Label = Label;
            newClone.Color = Color;
            newClone.SourcePin = SourcePin;
            if ((mSource != null))
            {
                newClone.mSource = ((ArticyValueArticyObject)(mSource.CloneObject()));
            }
        }
        
        public object CloneObject()
        {
            IncomingConnection clone = new IncomingConnection();
            CloneProperties(clone);
            return clone;
        }
        
        public virtual bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return false;
        }
    }
}
