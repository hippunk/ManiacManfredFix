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
    public class ZoneConditionFeature : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private ArticyValueArticyObject mIfConditionTrue = new ArticyValueArticyObject();
        
        [SerializeField()]
        private ArticyValueArticyObject mIfConditionFalse = new ArticyValueArticyObject();
        
        [SerializeField()]
        private MouseCursor mCursorIfConditionTrue = new MouseCursor();
        
        [SerializeField()]
        private MouseCursor mCursorIfConditionFalse = new MouseCursor();
        
        [SerializeField()]
        private ArticyValueArticyObject mItemToInteractWith = new ArticyValueArticyObject();
        
        [SerializeField()]
        private ArticyValueArticyObject mLinkIfItemValid = new ArticyValueArticyObject();
        
        [SerializeField()]
        private ArticyValueArticyObject mLinkIfItemInvalid = new ArticyValueArticyObject();
        
        [SerializeField()]
        private ArticyValueArticyScriptCondition mClickCondition = new ArticyValueArticyScriptCondition();
        
        [SerializeField()]
        private ArticyValueArticyScriptInstruction mOnClickInstruction = new ArticyValueArticyScriptInstruction();
        
        [SerializeField()]
        private ArticyValueArticyScriptCondition mInteractionCondition = new ArticyValueArticyScriptCondition();
        
        [SerializeField()]
        private ArticyValueArticyScriptInstruction mInstructionIfItemValid = new ArticyValueArticyScriptInstruction();
        
        public ArticyObject IfConditionTrue
        {
            get
            {
                return mIfConditionTrue.GetValue();
            }
            set
            {
                mIfConditionTrue.SetValue(value);
            }
        }
        
        public ArticyObject IfConditionFalse
        {
            get
            {
                return mIfConditionFalse.GetValue();
            }
            set
            {
                mIfConditionFalse.SetValue(value);
            }
        }
        
        public MouseCursor CursorIfConditionTrue
        {
            get
            {
                return mCursorIfConditionTrue;
            }
            set
            {
                mCursorIfConditionTrue = value;
            }
        }
        
        public MouseCursor CursorIfConditionFalse
        {
            get
            {
                return mCursorIfConditionFalse;
            }
            set
            {
                mCursorIfConditionFalse = value;
            }
        }
        
        public ArticyObject ItemToInteractWith
        {
            get
            {
                return mItemToInteractWith.GetValue();
            }
            set
            {
                mItemToInteractWith.SetValue(value);
            }
        }
        
        public ArticyObject LinkIfItemValid
        {
            get
            {
                return mLinkIfItemValid.GetValue();
            }
            set
            {
                mLinkIfItemValid.SetValue(value);
            }
        }
        
        public ArticyObject LinkIfItemInvalid
        {
            get
            {
                return mLinkIfItemInvalid.GetValue();
            }
            set
            {
                mLinkIfItemInvalid.SetValue(value);
            }
        }
        
        public ArticyScriptCondition ClickCondition
        {
            get
            {
                return mClickCondition.GetValue();
            }
            set
            {
                mClickCondition.SetValue(value);
            }
        }
        
        public ArticyScriptInstruction OnClickInstruction
        {
            get
            {
                return mOnClickInstruction.GetValue();
            }
            set
            {
                mOnClickInstruction.SetValue(value);
            }
        }
        
        public ArticyScriptCondition InteractionCondition
        {
            get
            {
                return mInteractionCondition.GetValue();
            }
            set
            {
                mInteractionCondition.SetValue(value);
            }
        }
        
        public ArticyScriptInstruction InstructionIfItemValid
        {
            get
            {
                return mInstructionIfItemValid.GetValue();
            }
            set
            {
                mInstructionIfItemValid.SetValue(value);
            }
        }
        
        private void CloneProperties(object aClone)
        {
            Articy.ManiacManfred.Features.ZoneConditionFeature newClone = ((Articy.ManiacManfred.Features.ZoneConditionFeature)(aClone));
            if ((mIfConditionTrue != null))
            {
                newClone.mIfConditionTrue = ((ArticyValueArticyObject)(mIfConditionTrue.CloneObject()));
            }
            if ((mIfConditionFalse != null))
            {
                newClone.mIfConditionFalse = ((ArticyValueArticyObject)(mIfConditionFalse.CloneObject()));
            }
            newClone.CursorIfConditionTrue = CursorIfConditionTrue;
            newClone.CursorIfConditionFalse = CursorIfConditionFalse;
            if ((mItemToInteractWith != null))
            {
                newClone.mItemToInteractWith = ((ArticyValueArticyObject)(mItemToInteractWith.CloneObject()));
            }
            if ((mLinkIfItemValid != null))
            {
                newClone.mLinkIfItemValid = ((ArticyValueArticyObject)(mLinkIfItemValid.CloneObject()));
            }
            if ((mLinkIfItemInvalid != null))
            {
                newClone.mLinkIfItemInvalid = ((ArticyValueArticyObject)(mLinkIfItemInvalid.CloneObject()));
            }
            if ((ClickCondition != null))
            {
                newClone.ClickCondition = ((ArticyScriptCondition)(ClickCondition.CloneObject()));
            }
            if ((OnClickInstruction != null))
            {
                newClone.OnClickInstruction = ((ArticyScriptInstruction)(OnClickInstruction.CloneObject()));
            }
            if ((InteractionCondition != null))
            {
                newClone.InteractionCondition = ((ArticyScriptCondition)(InteractionCondition.CloneObject()));
            }
            if ((InstructionIfItemValid != null))
            {
                newClone.InstructionIfItemValid = ((ArticyScriptInstruction)(InstructionIfItemValid.CloneObject()));
            }
        }
        
        public object CloneObject()
        {
            Articy.ManiacManfred.Features.ZoneConditionFeature clone = new Articy.ManiacManfred.Features.ZoneConditionFeature();
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
            if ((aProperty == "IfConditionTrue"))
            {
                IfConditionTrue = ((ArticyObject)(aValue));
                return;
            }
            if ((aProperty == "IfConditionFalse"))
            {
                IfConditionFalse = ((ArticyObject)(aValue));
                return;
            }
            if ((aProperty == "CursorIfConditionTrue"))
            {
                CursorIfConditionTrue = ((MouseCursor)(aValue));
                return;
            }
            if ((aProperty == "CursorIfConditionFalse"))
            {
                CursorIfConditionFalse = ((MouseCursor)(aValue));
                return;
            }
            if ((aProperty == "ItemToInteractWith"))
            {
                ItemToInteractWith = ((ArticyObject)(aValue));
                return;
            }
            if ((aProperty == "LinkIfItemValid"))
            {
                LinkIfItemValid = ((ArticyObject)(aValue));
                return;
            }
            if ((aProperty == "LinkIfItemInvalid"))
            {
                LinkIfItemInvalid = ((ArticyObject)(aValue));
                return;
            }
            if ((aProperty == "ClickCondition"))
            {
                ClickCondition = ((ArticyScriptCondition)(aValue));
                return;
            }
            if ((aProperty == "OnClickInstruction"))
            {
                OnClickInstruction = ((ArticyScriptInstruction)(aValue));
                return;
            }
            if ((aProperty == "InteractionCondition"))
            {
                InteractionCondition = ((ArticyScriptCondition)(aValue));
                return;
            }
            if ((aProperty == "InstructionIfItemValid"))
            {
                InstructionIfItemValid = ((ArticyScriptInstruction)(aValue));
                return;
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "IfConditionTrue"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(IfConditionTrue);
            }
            if ((aProperty == "IfConditionFalse"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(IfConditionFalse);
            }
            if ((aProperty == "CursorIfConditionTrue"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(CursorIfConditionTrue);
            }
            if ((aProperty == "CursorIfConditionFalse"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(CursorIfConditionFalse);
            }
            if ((aProperty == "ItemToInteractWith"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ItemToInteractWith);
            }
            if ((aProperty == "LinkIfItemValid"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(LinkIfItemValid);
            }
            if ((aProperty == "LinkIfItemInvalid"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(LinkIfItemInvalid);
            }
            if ((aProperty == "ClickCondition"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ClickCondition);
            }
            if ((aProperty == "OnClickInstruction"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(OnClickInstruction);
            }
            if ((aProperty == "InteractionCondition"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(InteractionCondition);
            }
            if ((aProperty == "InstructionIfItemValid"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(InstructionIfItemValid);
            }
            return null;
        }
        #endregion
    }
}
