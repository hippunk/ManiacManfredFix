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
    
    
    public class TextObject : ArticyObject, ITextObject, IPropertyProvider, IObjectWithColor, IObjectWithDisplayName, IObjectWithLocalizableDisplayName, IObjectWithPreviewImage, IObjectWithText, IObjectWithLocalizableText, IObjectWithAttachments, IObjectWithExternalId, IObjectWithShortId, IObjectWithPosition, IObjectWithZIndex, IObjectWithSize
    {
        
        [SerializeField()]
        private String mLocaKey_DisplayName;
        
        [SerializeField()]
        private String mOverwritten_DisplayName = "";
        
        [SerializeField()]
        private ArticyValueArticyModelList mAttachments = new ArticyValueArticyModelList();
        
        [SerializeField()]
        private PreviewImage mPreviewImage = new PreviewImage();
        
        [SerializeField()]
        private Color mColor;
        
        [SerializeField()]
        private String mLocaKey_Text;
        
        [SerializeField()]
        private String mOverwritten_Text = "";
        
        [SerializeField()]
        private String mExternalId;
        
        [SerializeField()]
        private Vector2 mPosition;
        
        [SerializeField()]
        private Single mZIndex;
        
        [SerializeField()]
        private Vector2 mSize;
        
        [SerializeField()]
        private UInt32 mShortId;
        
        public String LocaKey_DisplayName
        {
            get
            {
                return mLocaKey_DisplayName;
            }
        }
        
        public String DisplayName
        {
            get
            {
                if ((mOverwritten_DisplayName != ""))
                {
                    return mOverwritten_DisplayName;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_DisplayName);
            }
            set
            {
                mOverwritten_DisplayName = value;
            }
        }
        
        public List<ArticyObject> Attachments
        {
            get
            {
                return mAttachments.GetValue();
            }
            set
            {
                mAttachments.SetValue(value);
            }
        }
        
        public PreviewImage PreviewImage
        {
            get
            {
                return mPreviewImage;
            }
            set
            {
                mPreviewImage = value;
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
        
        public String LocaKey_Text
        {
            get
            {
                return mLocaKey_Text;
            }
        }
        
        public String Text
        {
            get
            {
                if ((mOverwritten_Text != ""))
                {
                    return mOverwritten_Text;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Text);
            }
            set
            {
                mOverwritten_Text = value;
            }
        }
        
        public String ExternalId
        {
            get
            {
                return mExternalId;
            }
            set
            {
                mExternalId = value;
            }
        }
        
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }
        
        public Single ZIndex
        {
            get
            {
                return mZIndex;
            }
            set
            {
                mZIndex = value;
            }
        }
        
        public Vector2 Size
        {
            get
            {
                return mSize;
            }
            set
            {
                mSize = value;
            }
        }
        
        public UInt32 ShortId
        {
            get
            {
                return mShortId;
            }
            set
            {
                mShortId = value;
            }
        }
        
        protected override void CloneProperties(object aClone)
        {
            TextObject newClone = ((TextObject)(aClone));
            newClone.mLocaKey_DisplayName = mLocaKey_DisplayName;
            newClone.mOverwritten_DisplayName = mOverwritten_DisplayName;
            mAttachments.CustomClone(newClone.mAttachments);
            newClone.PreviewImage = PreviewImage;
            newClone.Color = Color;
            newClone.mLocaKey_Text = mLocaKey_Text;
            newClone.mOverwritten_Text = mOverwritten_Text;
            newClone.ExternalId = ExternalId;
            newClone.Position = Position;
            newClone.ZIndex = ZIndex;
            newClone.Size = Size;
            newClone.ShortId = ShortId;
            base.CloneProperties(newClone);
        }
        
        public override bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            if ((mOverwritten_DisplayName != ""))
            {
                return true;
            }
            if ((mOverwritten_Text != ""))
            {
                return true;
            }
            return base.IsLocalizedPropertyOverwritten(aProperty);
        }
        
        #region property provider interface
        public override void setProp(string aProperty, object aValue)
        {
            if ((aProperty == "DisplayName"))
            {
                DisplayName = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Attachments"))
            {
                Attachments = ((List<ArticyObject>)(aValue));
                return;
            }
            if ((aProperty == "PreviewImage"))
            {
                PreviewImage = ((PreviewImage)(aValue));
                return;
            }
            if ((aProperty == "Color"))
            {
                Color = ((Color)(aValue));
                return;
            }
            if ((aProperty == "Text"))
            {
                Text = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "ExternalId"))
            {
                ExternalId = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Position"))
            {
                Position = ((Vector2)(aValue));
                return;
            }
            if ((aProperty == "ZIndex"))
            {
                ZIndex = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "Size"))
            {
                Size = ((Vector2)(aValue));
                return;
            }
            if ((aProperty == "ShortId"))
            {
                ShortId = ((UInt32)(aValue));
                return;
            }
            base.setProp(aProperty, aValue);
        }
        
        public override Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "DisplayName"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(DisplayName);
            }
            if ((aProperty == "Attachments"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Attachments);
            }
            if ((aProperty == "PreviewImage"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(PreviewImage);
            }
            if ((aProperty == "Color"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Color);
            }
            if ((aProperty == "Text"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Text);
            }
            if ((aProperty == "ExternalId"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ExternalId);
            }
            if ((aProperty == "Position"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Position);
            }
            if ((aProperty == "ZIndex"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ZIndex);
            }
            if ((aProperty == "Size"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Size);
            }
            if ((aProperty == "ShortId"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ShortId);
            }
            return base.getProp(aProperty);
        }
        #endregion
    }
}
