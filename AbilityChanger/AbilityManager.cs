
using static Satchel.FsmUtil;

namespace AbilityChanger
{
    public abstract class AbilityManager{
        protected List<Ability> options;
        internal string currentlySelected;
        protected GameObject InvGo;
        public abstract string abilityName { get; protected set; }
        public abstract string inventoryTitleKey { get; protected set; }
        public abstract string inventoryDescKey { get; protected set; }

        public abstract bool hasDefaultAbility();
        public void addAbility(Ability ability){
            options.Add(ability);
        }
        public void removeAbility(string abilityName){
            options = options.Where( a => a.name != abilityName).ToList();
        }        
        public bool isCustom(){
            return hasAcquiredAbility() && getAbility().isCustom;
        }
        public Ability getAbility(){
            var validOptions = acquiredAbilities();
            return validOptions.FirstOrDefault(a => a.name == currentlySelected) ?? validOptions[0];
        }

        public Ability getDefaultAbility(){
            return options.First(a => a.isCustom == false);
        }

        public Ability nextAbility(){
            var validOptions = acquiredAbilities();
            var currentIndex = validOptions.FindIndex(a => a.name == currentlySelected);
            Ability nextAbility;
            if(validOptions.Count() > currentIndex + 1){
               nextAbility = validOptions[currentIndex + 1];
            } else {
               nextAbility = validOptions[0];
            }
            return nextAbility;
        }

        public bool hasAcquiredAbility(){
            return options.Any( a => a.hasAbility());
        }

        public List<Ability> acquiredAbilities(){
            return options.Where( a => a.hasAbility()).ToList();
        }

        public AbilityManager(){
            options = new(){new DefaultAbility(abilityName,hasDefaultAbility)};
            currentlySelected = abilityName;
            On.PlayMakerFSM.OnEnable += InventoryManagement;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            ModHooks.LanguageGetHook += LanguageGet;
        }
        public abstract GameObject getIconGo();
        public virtual void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self){
            orig(self);
        }

        public virtual void updateIcon(GameObject icon){
            var currentAbility = getAbility();
            var defaultAbility = getDefaultAbility();
            var itemdisplay = icon.GetComponent<InvItemDisplay>();
            if(itemdisplay != null){
                if(defaultAbility.activeSprite == null){
                    defaultAbility.activeSprite = itemdisplay.activeSprite;
                }
                if(defaultAbility.inactiveSprite == null){
                    defaultAbility.inactiveSprite = itemdisplay.inactiveSprite;
                }
                if(currentAbility.activeSprite != null){
                    
                    itemdisplay.activeSprite = currentAbility.activeSprite;
                }
                if(currentAbility.inactiveSprite != null){
                    itemdisplay.inactiveSprite = currentAbility.inactiveSprite;
                }
                
                itemdisplay.SendMessage("OnEnable");
            } else {
                var spriteRenderer = icon.GetComponent<SpriteRenderer>();
                if(defaultAbility.activeSprite == null){
                    defaultAbility.activeSprite = spriteRenderer.sprite;
                }
                if(currentAbility.activeSprite != null){
                    spriteRenderer.sprite = currentAbility.activeSprite;
                }
            }
        }

        private void updateText(){
            if(InvGo != null){
                InvGo.LocateMyFSM("Update Text").Fsm.Event("UPDATE TEXT");
            }
        }
        private void InventoryManagement(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "Update Text")
            {
                InvGo = self.gameObject;
            }
            if (self.gameObject.name == "Inventory" && self.FsmName == "Inventory Control")
            {
                self.AddCustomAction("Opened",() => {
                    if(hasAcquiredAbility()){
                        updateInventory();
                    }
                });
            }
        }
        public void updateInventory(){
            updateText();
            var icon = getIconGo();
            if(icon != null){
                updateIcon(icon);
            }
        }
        private string LanguageGet(string title, string sheet, string orig){
            if(sheet == "UI" && title == inventoryTitleKey && isCustom()){
                   return getAbility().title;
            }
            
            if(sheet == "UI" && title == inventoryDescKey && options.Count() > 1){
                var final = orig;
                if(isCustom()){
                    final = getAbility().description;
                }
                return $"Press the confirm button to cycle abilities. <br><br> {final}";
            }
            return orig;
        }
        public void handleAbilityUse(string interceptedState = "",string interceptedEvent = ""){
            getAbility().handleAbilityUse(interceptedState,interceptedEvent);
        }

    }
}