!function(e,t){"object"==typeof exports&&"undefined"!=typeof module?module.exports=t():"function"==typeof define&&define.amd?define(t):e.Sweetalert2=t()}(this,function(){"use strict";var e="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(e){return typeof e}:function(e){return e&&"function"==typeof Symbol&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e},t=function(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")},n=function(){function e(e,t){for(var n=0;n<t.length;n++){var o=t[n];o.enumerable=o.enumerable||!1,o.configurable=!0,"value"in o&&(o.writable=!0),Object.defineProperty(e,o.key,o)}}return function(t,n,o){return n&&e(t.prototype,n),o&&e(t,o),t}}(),o=Object.assign||function(e){for(var t=1;t<arguments.length;t++){var n=arguments[t];for(var o in n)Object.prototype.hasOwnProperty.call(n,o)&&(e[o]=n[o])}return e},r=function e(t,n,o){null===t&&(t=Function.prototype);var r=Object.getOwnPropertyDescriptor(t,n);if(void 0===r){var i=Object.getPrototypeOf(t);return null===i?void 0:e(i,n,o)}if("value"in r)return r.value;var a=r.get;return void 0!==a?a.call(o):void 0},i=function(e,t){if("function"!=typeof t&&null!==t)throw new TypeError("Super expression must either be null or a function, not "+typeof t);e.prototype=Object.create(t&&t.prototype,{constructor:{value:e,enumerable:!1,writable:!0,configurable:!0}}),t&&(Object.setPrototypeOf?Object.setPrototypeOf(e,t):e.__proto__=t)},a=function(e,t){if(!e)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return!t||"object"!=typeof t&&"function"!=typeof t?e:t},s=function(e,t){if(Array.isArray(e))return e;if(Symbol.iterator in Object(e))return function(e,t){var n=[],o=!0,r=!1,i=void 0;try{for(var a,s=e[Symbol.iterator]();!(o=(a=s.next()).done)&&(n.push(a.value),!t||n.length!==t);o=!0);}catch(e){r=!0,i=e}finally{try{!o&&s.return&&s.return()}finally{if(r)throw i}}return n}(e,t);throw new TypeError("Invalid attempt to destructure non-iterable instance")},u="SweetAlert2:",c=function(e){return Array.prototype.slice.call(e)},l=function(e){console.warn(u+" "+e)},d=function(e){console.error(u+" "+e)},p=[],f=function(e){-1===p.indexOf(e)&&(p.push(e),l(e))},m=function(e){return"function"==typeof e?e():e},h=function(t){return"object"===(void 0===t?"undefined":e(t))&&"function"==typeof t.then},g=Object.freeze({cancel:"cancel",backdrop:"overlay",close:"close",esc:"esc",timer:"timer"}),v=function(e){var t={};for(var n in e)t[e[n]]="swal2-"+e[n];return t},y=v(["container","shown","height-auto","iosfix","popup","modal","no-backdrop","toast","toast-shown","toast-column","fade","show","hide","noanimation","close","title","header","content","actions","confirm","cancel","footer","icon","icon-text","image","input","file","range","select","radio","checkbox","label","textarea","inputerror","validationerror","progresssteps","activeprogressstep","progresscircle","progressline","loading","styled","top","top-start","top-end","top-left","top-right","center","center-start","center-end","center-left","center-right","bottom","bottom-start","bottom-end","bottom-left","bottom-right","grow-row","grow-column","grow-fullscreen"]),b=v(["success","warning","info","question","error"]),w={previousBodyPadding:null},C=function(e,t){return!!e.classList&&e.classList.contains(t)},k=function(e){if(e.focus(),"file"!==e.type){var t=e.value;e.value="",e.value=t}},x=function(e,t,n){e&&t&&("string"==typeof t&&(t=t.split(/\s+/).filter(Boolean)),t.forEach(function(t){e.forEach?e.forEach(function(e){n?e.classList.add(t):e.classList.remove(t)}):n?e.classList.add(t):e.classList.remove(t)}))},B=function(e,t){x(e,t,!0)},A=function(e,t){x(e,t,!1)},S=function(e,t){for(var n=0;n<e.childNodes.length;n++)if(C(e.childNodes[n],t))return e.childNodes[n]},P=function(e){e.style.opacity="",e.style.display=e.id===y.content?"block":"flex"},E=function(e){e.style.opacity="",e.style.display="none"},O=function(e){return e&&(e.offsetWidth||e.offsetHeight||e.getClientRects().length)},L=function(){return document.body.querySelector("."+y.container)},T=function(e){var t=L();return t?t.querySelector("."+e):null},j=function(){return T(y.popup)},_=function(){var e=j();return c(e.querySelectorAll("."+y.icon))},V=function(){return T(y.title)},q=function(){return T(y.content)},M=function(){return T(y.image)},I=function(){return T(y.progresssteps)},H=function(){return T(y.confirm)},R=function(){return T(y.cancel)},D=function(){return T(y.actions)},N=function(){return T(y.footer)},W=function(){return T(y.close)},z=function(){var e=c(j().querySelectorAll('[tabindex]:not([tabindex="-1"]):not([tabindex="0"])')).sort(function(e,t){return e=parseInt(e.getAttribute("tabindex")),(t=parseInt(t.getAttribute("tabindex")))<e?1:e<t?-1:0}),t=c(j().querySelectorAll('a[href], area[href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), iframe, object, embed, [tabindex="0"], [contenteditable], audio[controls], video[controls]'));return function(e){for(var t=[],n=0;n<e.length;n++)-1===t.indexOf(e[n])&&t.push(e[n]);return t}(e.concat(t))},U=function(){return!document.body.classList.contains(y["toast-shown"])},F=function(){return document.body.classList.contains(y["toast-shown"])},K=function(){return"undefined"==typeof window||"undefined"==typeof document},Z=('\n <div aria-labelledby="'+y.title+'" aria-describedby="'+y.content+'" class="'+y.popup+'" tabindex="-1">\n   <div class="'+y.header+'">\n     <ul class="'+y.progresssteps+'"></ul>\n     <div class="'+y.icon+" "+b.error+'">\n       <span class="swal2-x-mark"><span class="swal2-x-mark-line-left"></span><span class="swal2-x-mark-line-right"></span></span>\n     </div>\n     <div class="'+y.icon+" "+b.question+'">\n       <span class="'+y["icon-text"]+'">?</span>\n      </div>\n     <div class="'+y.icon+" "+b.warning+'">\n       <span class="'+y["icon-text"]+'">!</span>\n      </div>\n     <div class="'+y.icon+" "+b.info+'">\n       <span class="'+y["icon-text"]+'">i</span>\n      </div>\n     <div class="'+y.icon+" "+b.success+'">\n       <div class="swal2-success-circular-line-left"></div>\n       <span class="swal2-success-line-tip"></span> <span class="swal2-success-line-long"></span>\n       <div class="swal2-success-ring"></div> <div class="swal2-success-fix"></div>\n       <div class="swal2-success-circular-line-right"></div>\n     </div>\n     <img class="'+y.image+'" />\n     <h2 class="'+y.title+'" id="'+y.title+'"></h2>\n     <button type="button" class="'+y.close+'">\xd7</button>\n   </div>\n   <div class="'+y.content+'">\n     <div id="'+y.content+'"></div>\n     <input class="'+y.input+'" />\n     <input type="file" class="'+y.file+'" />\n     <div class="'+y.range+'">\n       <input type="range" />\n       <output></output>\n     </div>\n     <select class="'+y.select+'"></select>\n     <div class="'+y.radio+'"></div>\n     <label for="'+y.checkbox+'" class="'+y.checkbox+'">\n       <input type="checkbox" />\n     </label>\n     <textarea class="'+y.textarea+'"></textarea>\n     <div class="'+y.validationerror+'" id="'+y.validationerror+'"></div>\n   </div>\n   <div class="'+y.actions+'">\n     <button type="button" class="'+y.confirm+'">OK</button>\n     <button type="button" class="'+y.cancel+'">Cancel</button>\n   </div>\n   <div class="'+y.footer+'">\n   </div>\n </div>\n').replace(/(^|\n)\s*/g,""),Q=function(e){var t=L();if(t&&(t.parentNode.removeChild(t),A([document.documentElement,document.body],[y["no-backdrop"],y["toast-shown"],y["has-column"]])),!K()){var n=document.createElement("div");n.className=y.container,n.innerHTML=Z,("string"==typeof e.target?document.querySelector(e.target):e.target).appendChild(n);var o=j(),r=q(),i=S(r,y.input),a=S(r,y.file),s=r.querySelector("."+y.range+" input"),u=r.querySelector("."+y.range+" output"),c=S(r,y.select),l=r.querySelector("."+y.checkbox+" input"),p=S(r,y.textarea);o.setAttribute("role",e.toast?"alert":"dialog"),o.setAttribute("aria-live",e.toast?"polite":"assertive"),e.toast||o.setAttribute("aria-modal","true");var f=void 0,m=function(e){xe.isVisible()&&f!==e.target.value&&xe.resetValidationError(),f=e.target.value};return i.oninput=m,a.onchange=m,c.onchange=m,l.onchange=m,p.oninput=m,s.oninput=function(e){m(e),u.value=s.value},s.onchange=function(e){m(e),s.nextSibling.value=s.value},o}d("SweetAlert2 requires document to initialize")},Y=function(t,n){if(!t)return E(n);if("object"===(void 0===t?"undefined":e(t)))if(n.innerHTML="",0 in t)for(var o=0;o in t;o++)n.appendChild(t[o].cloneNode(!0));else n.appendChild(t.cloneNode(!0));else t&&(n.innerHTML=t);P(n)},$=function(){if(K())return!1;var e=document.createElement("div"),t={WebkitAnimation:"webkitAnimationEnd",OAnimation:"oAnimationEnd oanimationend",animation:"animationend"};for(var n in t)if(t.hasOwnProperty(n)&&void 0!==e.style[n])return t[n];return!1}(),J=function(e){var t=I(),n=parseInt(null===e.currentProgressStep?xe.getQueueStep():e.currentProgressStep,10);e.progressSteps&&e.progressSteps.length?(P(t),function(e){for(;e.firstChild;)e.removeChild(e.firstChild)}(t),n>=e.progressSteps.length&&l("Invalid currentProgressStep parameter, it should be less than progressSteps.length (currentProgressStep like JS arrays starts from 0)"),e.progressSteps.forEach(function(o,r){var i=document.createElement("li");if(B(i,y.progresscircle),i.innerHTML=o,r===n&&B(i,y.activeprogressstep),t.appendChild(i),r!==e.progressSteps.length-1){var a=document.createElement("li");B(a,y.progressline),e.progressStepsDistance&&(a.style.width=e.progressStepsDistance),t.appendChild(a)}})):E(t)},X={},G=function(e,t){var n=L(),o=j();if(o){null!==e&&"function"==typeof e&&e(o),A(o,y.show),B(o,y.hide);var r=function(){var e,o;F()||(e=window.scrollX,o=window.scrollY,X.restoreFocusTimeout=setTimeout(function(){X.previousActiveElement&&X.previousActiveElement.focus&&(X.previousActiveElement.focus(),X.previousActiveElement=null)},100),void 0!==e&&void 0!==o&&window.scrollTo(e,o),X.keydownTarget.removeEventListener("keydown",X.keydownHandler,{capture:X.keydownListenerCapture}),X.keydownHandlerAdded=!1),n.parentNode&&n.parentNode.removeChild(n),A([document.documentElement,document.body],[y.shown,y["height-auto"],y["no-backdrop"],y["toast-shown"],y["toast-column"]]),U()&&(null!==w.previousBodyPadding&&(document.body.style.paddingRight=w.previousBodyPadding,w.previousBodyPadding=null),function(){if(C(document.body,y.iosfix)){var e=parseInt(document.body.style.top,10);A(document.body,y.iosfix),document.body.style.top="",document.body.scrollTop=-1*e}}()),null!==t&&"function"==typeof t&&setTimeout(function(){t()})};$&&!C(o,y.noanimation)?o.addEventListener($,function e(){o.removeEventListener($,e),C(o,y.hide)&&r()}):r()}};function ee(e){var t=function e(){for(var t=arguments.length,n=Array(t),o=0;o<t;o++)n[o]=arguments[o];if(!(this instanceof e))return new(Function.prototype.bind.apply(e,[null].concat(n)));Object.getPrototypeOf(e).apply(this,n)};return t.prototype=o(Object.create(e.prototype),{constructor:t}),"function"==typeof Object.setPrototypeOf?Object.setPrototypeOf(t,e):t.__proto__=e,t}var te={title:"",titleText:"",text:"",html:"",footer:"",type:null,toast:!1,customClass:"",target:"body",backdrop:!0,animation:!0,heightAuto:!0,allowOutsideClick:!0,allowEscapeKey:!0,allowEnterKey:!0,stopKeydownPropagation:!0,keydownListenerCapture:!1,showConfirmButton:!0,showCancelButton:!1,preConfirm:null,confirmButtonText:"OK",confirmButtonAriaLabel:"",confirmButtonColor:null,confirmButtonClass:null,cancelButtonText:"Cancel",cancelButtonAriaLabel:"",cancelButtonColor:null,cancelButtonClass:null,buttonsStyling:!0,reverseButtons:!1,focusConfirm:!0,focusCancel:!1,showCloseButton:!1,closeButtonAriaLabel:"Close this dialog",showLoaderOnConfirm:!1,imageUrl:null,imageWidth:null,imageHeight:null,imageAlt:"",imageClass:null,timer:null,width:null,padding:null,background:null,input:null,inputPlaceholder:"",inputValue:"",inputOptions:{},inputAutoTrim:!0,inputClass:null,inputAttributes:{},inputValidator:null,grow:!1,position:"center",progressSteps:[],currentProgressStep:null,progressStepsDistance:null,onBeforeOpen:null,onAfterClose:null,onOpen:null,onClose:null,useRejections:!1,expectRejections:!1},ne=["useRejections","expectRejections"],oe=function(e){return te.hasOwnProperty(e)||"extraParams"===e},re=function(e){return-1!==ne.indexOf(e)},ie=function(e){for(var t in e)oe(t)||l('Unknown parameter "'+t+'"'),re(t)&&f('The parameter "'+t+'" is deprecated and will be removed in the next major release.')},ae='"setDefaults" & "resetDefaults" methods are deprecated in favor of "mixin" method and will be removed in the next major release. For new projects, use "mixin". For past projects already using "setDefaults", support will be provided through an additional package.',se={},ue=[],ce=function(){var e=j();e||xe(""),e=j();var t=D(),n=H(),o=R();P(t),P(n),B([e,t],y.loading),n.disabled=!0,o.disabled=!0,e.setAttribute("data-loading",!0),e.setAttribute("aria-busy",!0),e.focus()},le=Object.freeze({isValidParameter:oe,isDeprecatedParameter:re,argsToParams:function(t){var n={};switch(e(t[0])){case"string":["title","html","type"].forEach(function(o,r){switch(e(t[r])){case"string":n[o]=t[r];break;case"undefined":break;default:d("Unexpected type of "+o+'! Expected "string", got '+e(t[r]))}});break;case"object":o(n,t[0]);break;default:return d('Unexpected type of argument! Expected "string" or "object", got '+e(t[0])),!1}return n},adaptInputValidator:function(e){return function(t,n){return e.call(this,t,n).then(function(){},function(e){return e})}},close:G,closePopup:G,closeModal:G,closeToast:G,isVisible:function(){return!!j()},clickConfirm:function(){return H().click()},clickCancel:function(){return R().click()},getPopup:j,getTitle:V,getContent:q,getImage:M,getIcons:_,getButtonsWrapper:function(){return f("swal.getButtonsWrapper() is deprecated and will be removed in the next major release, use swal.getActions() instead"),T(y.actions)},getActions:D,getConfirmButton:H,getCancelButton:R,getFooter:N,isLoading:function(){return j().hasAttribute("data-loading")},fire:function(){for(var e=arguments.length,t=Array(e),n=0;n<e;n++)t[n]=arguments[n];return new(Function.prototype.bind.apply(this,[null].concat(t)))},mixin:function(e){return ee(function(s){function u(){return t(this,u),a(this,(u.__proto__||Object.getPrototypeOf(u)).apply(this,arguments))}return i(u,s),n(u,[{key:"_main",value:function(t){return r(u.prototype.__proto__||Object.getPrototypeOf(u.prototype),"_main",this).call(this,o({},e,t))}}]),u}(this))},queue:function(e){var t=this;ue=e;var n=function(){ue=[],document.body.removeAttribute("data-swal2-queue-step")},o=[];return new Promise(function(e,r){!function r(i,a){i<ue.length?(document.body.setAttribute("data-swal2-queue-step",i),t(ue[i]).then(function(t){void 0!==t.value?(o.push(t.value),r(i+1,a)):(n(),e({dismiss:t.dismiss}))})):(n(),e({value:o}))}(0)})},getQueueStep:function(){return document.body.getAttribute("data-swal2-queue-step")},insertQueueStep:function(e,t){return t&&t<ue.length?ue.splice(t,0,e):ue.push(e)},deleteQueueStep:function(e){void 0!==ue[e]&&ue.splice(e,1)},showLoading:ce,enableLoading:ce,getTimerLeft:function(){return X.timeout&&X.timeout.getTimerLeft()}}),de="function"==typeof Symbol?Symbol:function(){var e=0;function t(t){return"__"+t+"_"+Math.floor(1e9*Math.random())+"_"+ ++e+"__"}return t.iterator=t("Symbol.iterator"),t}(),pe="function"==typeof WeakMap?WeakMap:function(e,t,n){function o(){t(this,e,{value:de("WeakMap")})}return o.prototype={delete:function(t){delete t[this[e]]},get:function(t){return t[this[e]]},has:function(t){return n.call(t,this[e])},set:function(n,o){t(n,this[e],{configurable:!0,value:o})}},o}(de("WeakMap"),Object.defineProperty,{}.hasOwnProperty),fe={promise:new pe,innerParams:new pe,domCache:new pe};function me(){var e=fe.innerParams.get(this),t=fe.domCache.get(this);e.showConfirmButton||(E(t.confirmButton),e.showCancelButton||E(t.actions)),A([t.popup,t.actions],y.loading),t.popup.removeAttribute("aria-busy"),t.popup.removeAttribute("data-loading"),t.confirmButton.disabled=!1,t.cancelButton.disabled=!1}var he=function e(n,o){var r,i,a;t(this,e);var s=o;this.start=function(){a=!0,i=new Date,r=setTimeout(n,s)},this.stop=function(){a=!1,clearTimeout(r),s-=new Date-i},this.getTimerLeft=function(){return a&&(this.stop(),this.start()),s},this.getStateRunning=function(){return a},this.start()},ge={email:function(e,t){return/^[a-zA-Z0-9.+_-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9-]{2,24}$/.test(e)?Promise.resolve():Promise.reject(t&&t.validationMessage?t.validationMessage:"Invalid email address")},url:function(e,t){return/^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_+.~#?&//=]*)$/.test(e)?Promise.resolve():Promise.reject(t&&t.validationMessage?t.validationMessage:"Invalid URL")}},ve=function(e){var t=L(),n=j();null!==e.onBeforeOpen&&"function"==typeof e.onBeforeOpen&&e.onBeforeOpen(n),e.animation?(B(n,y.show),B(t,y.fade),A(n,y.hide)):A(n,y.fade),P(n),t.style.overflowY="hidden",$&&!C(n,y.noanimation)?n.addEventListener($,function e(){n.removeEventListener($,e),t.style.overflowY="auto"}):t.style.overflowY="auto",B([document.documentElement,document.body,t],y.shown),e.heightAuto&&e.backdrop&&!e.toast&&B([document.documentElement,document.body],y["height-auto"]),U()&&(null===w.previousBodyPadding&&document.body.scrollHeight>window.innerHeight&&(w.previousBodyPadding=parseInt(window.getComputedStyle(document.body).getPropertyValue("padding-right")),document.body.style.paddingRight=w.previousBodyPadding+function(){if("ontouchstart"in window||navigator.msMaxTouchPoints)return 0;var e=document.createElement("div");e.style.width="50px",e.style.height="50px",e.style.overflow="scroll",document.body.appendChild(e);var t=e.offsetWidth-e.clientWidth;return document.body.removeChild(e),t}()+"px"),function(){if(/iPad|iPhone|iPod/.test(navigator.userAgent)&&!window.MSStream&&!C(document.body,y.iosfix)){var e=document.body.scrollTop;document.body.style.top=-1*e+"px",B(document.body,y.iosfix)}}()),F()||X.previousActiveElement||(X.previousActiveElement=document.activeElement),null!==e.onOpen&&"function"==typeof e.onOpen&&setTimeout(function(){e.onOpen(n)})},ye=Object.freeze({hideLoading:me,disableLoading:me,getInput:function(e){var t=fe.innerParams.get(this),n=fe.domCache.get(this);if(!(e=e||t.input))return null;switch(e){case"select":case"textarea":case"file":return S(n.content,y[e]);case"checkbox":return n.popup.querySelector("."+y.checkbox+" input");case"radio":return n.popup.querySelector("."+y.radio+" input:checked")||n.popup.querySelector("."+y.radio+" input:first-child");case"range":return n.popup.querySelector("."+y.range+" input");default:return S(n.content,y.input)}},enableButtons:function(){var e=fe.domCache.get(this);e.confirmButton.disabled=!1,e.cancelButton.disabled=!1},disableButtons:function(){var e=fe.domCache.get(this);e.confirmButton.disabled=!0,e.cancelButton.disabled=!0},enableConfirmButton:function(){fe.domCache.get(this).confirmButton.disabled=!1},disableConfirmButton:function(){fe.domCache.get(this).confirmButton.disabled=!0},enableInput:function(){var e=this.getInput();if(!e)return!1;if("radio"===e.type)for(var t=e.parentNode.parentNode.querySelectorAll("input"),n=0;n<t.length;n++)t[n].disabled=!1;else e.disabled=!1},disableInput:function(){var e=this.getInput();if(!e)return!1;if(e&&"radio"===e.type)for(var t=e.parentNode.parentNode.querySelectorAll("input"),n=0;n<t.length;n++)t[n].disabled=!0;else e.disabled=!0},showValidationError:function(e){var t=fe.domCache.get(this);t.validationError.innerHTML=e;var n=window.getComputedStyle(t.popup);t.validationError.style.marginLeft="-"+n.getPropertyValue("padding-left"),t.validationError.style.marginRight="-"+n.getPropertyValue("padding-right"),P(t.validationError);var o=this.getInput();o&&(o.setAttribute("aria-invalid",!0),o.setAttribute("aria-describedBy",y.validationerror),k(o),B(o,y.inputerror))},resetValidationError:function(){var e=fe.domCache.get(this);e.validationError&&E(e.validationError);var t=this.getInput();t&&(t.removeAttribute("aria-invalid"),t.removeAttribute("aria-describedBy"),A(t,y.inputerror))},getProgressSteps:function(){return fe.innerParams.get(this).progressSteps},setProgressSteps:function(e){var t=fe.innerParams.get(this),n=o({},t,{progressSteps:e});fe.innerParams.set(this,n),J(n)},showProgressSteps:function(){var e=fe.domCache.get(this);P(e.progressSteps)},hideProgressSteps:function(){var e=fe.domCache.get(this);E(e.progressSteps)},_main:function(t){var n=this;ie(t);var r=o({},te,t);!function(e){e.inputValidator||Object.keys(ge).forEach(function(t){e.input===t&&(e.inputValidator=e.expectRejections?ge[t]:xe.adaptInputValidator(ge[t]))}),(!e.target||"string"==typeof e.target&&!document.querySelector(e.target)||"string"!=typeof e.target&&!e.target.appendChild)&&(l('Target parameter is not valid, defaulting to "body"'),e.target="body");var t=void 0,n=j(),o="string"==typeof e.target?document.querySelector(e.target):e.target;t=n&&o&&n.parentNode!==o.parentNode?Q(e):n||Q(e),e.width&&(t.style.width="number"==typeof e.width?e.width+"px":e.width),e.padding&&(t.style.padding="number"==typeof e.padding?e.padding+"px":e.padding),e.background&&(t.style.background=e.background);for(var r=window.getComputedStyle(t).getPropertyValue("background-color"),i=t.querySelectorAll("[class^=swal2-success-circular-line], .swal2-success-fix"),a=0;a<i.length;a++)i[a].style.backgroundColor=r;var s=L(),u=V(),c=q().querySelector("#"+y.content),p=D(),f=H(),m=R(),h=W(),g=N();if(e.titleText?u.innerText=e.titleText:e.title&&("string"==typeof e.title&&(e.title=e.title.split("\n").join("<br />")),Y(e.title,u)),"string"==typeof e.backdrop?L().style.background=e.backdrop:e.backdrop||B([document.documentElement,document.body],y["no-backdrop"]),e.html?Y(e.html,c):e.text?(c.textContent=e.text,P(c)):E(c),e.position in y?B(s,y[e.position]):(l('The "position" parameter is not valid, defaulting to "center"'),B(s,y.center)),e.grow&&"string"==typeof e.grow){var v="grow-"+e.grow;v in y&&B(s,y[v])}"function"==typeof e.animation&&(e.animation=e.animation.call()),e.showCloseButton?(h.setAttribute("aria-label",e.closeButtonAriaLabel),P(h)):E(h),t.className=y.popup,e.toast?(B([document.documentElement,document.body],y["toast-shown"]),B(t,y.toast)):B(t,y.modal),e.customClass&&B(t,e.customClass),J(e);for(var w=_(),C=0;C<w.length;C++)E(w[C]);if(e.type){var k=!1;for(var x in b)if(e.type===x){k=!0;break}if(!k)return d("Unknown alert type: "+e.type);var S=t.querySelector("."+y.icon+"."+b[e.type]);P(S),e.animation&&B(S,"swal2-animate-"+e.type+"-icon")}var O=M();if(e.imageUrl?(O.setAttribute("src",e.imageUrl),O.setAttribute("alt",e.imageAlt),P(O),e.imageWidth?O.setAttribute("width",e.imageWidth):O.removeAttribute("width"),e.imageHeight?O.setAttribute("height",e.imageHeight):O.removeAttribute("height"),O.className=y.image,e.imageClass&&B(O,e.imageClass)):E(O),e.showCancelButton?m.style.display="inline-block":E(m),e.showConfirmButton?function(e,t){e.style.removeProperty?e.style.removeProperty(t):e.style.removeAttribute(t)}(f,"display"):E(f),e.showConfirmButton||e.showCancelButton?P(p):E(p),f.innerHTML=e.confirmButtonText,m.innerHTML=e.cancelButtonText,f.setAttribute("aria-label",e.confirmButtonAriaLabel),m.setAttribute("aria-label",e.cancelButtonAriaLabel),f.className=y.confirm,B(f,e.confirmButtonClass),m.className=y.cancel,B(m,e.cancelButtonClass),e.buttonsStyling){B([f,m],y.styled),e.confirmButtonColor&&(f.style.backgroundColor=e.confirmButtonColor),e.cancelButtonColor&&(m.style.backgroundColor=e.cancelButtonColor);var T=window.getComputedStyle(f).getPropertyValue("background-color");f.style.borderLeftColor=T,f.style.borderRightColor=T}else A([f,m],y.styled),f.style.backgroundColor=f.style.borderLeftColor=f.style.borderRightColor="",m.style.backgroundColor=m.style.borderLeftColor=m.style.borderRightColor="";Y(e.footer,g),!0===e.animation?A(t,y.noanimation):B(t,y.noanimation),e.showLoaderOnConfirm&&!e.preConfirm&&l("showLoaderOnConfirm is set to true, but preConfirm is not defined.\nshowLoaderOnConfirm should be used together with preConfirm, see usage example:\nhttps://sweetalert2.github.io/#ajax-request")}(r),Object.freeze(r),fe.innerParams.set(this,r),X.timeout&&(X.timeout.stop(),delete X.timeout),clearTimeout(X.restoreFocusTimeout);var i={popup:j(),container:L(),content:q(),actions:D(),confirmButton:H(),cancelButton:R(),closeButton:W(),validationError:T(y.validationerror),progressSteps:I()};fe.domCache.set(this,i);var a=this.constructor;return new Promise(function(t,o){var u=function(e){a.closePopup(r.onClose,r.onAfterClose),t(r.useRejections?e:{value:e})},c=function(e){a.closePopup(r.onClose,r.onAfterClose),r.useRejections?o(e):t({dismiss:e})},l=function(e){a.closePopup(r.onClose,r.onAfterClose),o(e)};r.timer&&(X.timeout=new he(function(){c("timer"),delete X.timeout},r.timer)),r.input&&setTimeout(function(){var e=n.getInput();e&&k(e)},0);for(var p=function(e){if(r.showLoaderOnConfirm&&a.showLoading(),r.preConfirm){n.resetValidationError();var t=Promise.resolve().then(function(){return r.preConfirm(e,r.extraParams)});r.expectRejections?t.then(function(t){return u(t||e)},function(e){n.hideLoading(),e&&n.showValidationError(e)}):t.then(function(t){O(i.validationError)||!1===t?n.hideLoading():u(t||e)},function(e){return l(e)})}else u(e)},f=function(e){var t=e||window.event,o=t.target||t.srcElement,s=i.confirmButton,u=i.cancelButton,d=s&&(s===o||s.contains(o)),f=u&&(u===o||u.contains(o));switch(t.type){case"click":if(d&&a.isVisible())if(n.disableButtons(),r.input){var m=function(){var e=n.getInput();if(!e)return null;switch(r.input){case"checkbox":return e.checked?1:0;case"radio":return e.checked?e.value:null;case"file":return e.files.length?e.files[0]:null;default:return r.inputAutoTrim?e.value.trim():e.value}}();if(r.inputValidator){n.disableInput();var h=Promise.resolve().then(function(){return r.inputValidator(m,r.extraParams)});r.expectRejections?h.then(function(){n.enableButtons(),n.enableInput(),p(m)},function(e){n.enableButtons(),n.enableInput(),e&&n.showValidationError(e)}):h.then(function(e){n.enableButtons(),n.enableInput(),e?n.showValidationError(e):p(m)},function(e){return l(e)})}else p(m)}else p(!0);else f&&a.isVisible()&&(n.disableButtons(),c(a.DismissReason.cancel))}},g=i.popup.querySelectorAll("button"),v=0;v<g.length;v++)g[v].onclick=f,g[v].onmouseover=f,g[v].onmouseout=f,g[v].onmousedown=f;if(i.closeButton.onclick=function(){c(a.DismissReason.close)},r.toast)i.popup.onclick=function(e){r.showConfirmButton||r.showCancelButton||r.showCloseButton||r.input||c(a.DismissReason.close)};else{var b=!1;i.popup.onmousedown=function(){i.container.onmouseup=function(e){i.container.onmouseup=void 0,e.target===i.container&&(b=!0)}},i.container.onmousedown=function(){i.popup.onmouseup=function(e){i.popup.onmouseup=void 0,(e.target===i.popup||i.popup.contains(e.target))&&(b=!0)}},i.container.onclick=function(e){b?b=!1:e.target===i.container&&m(r.allowOutsideClick)&&c(a.DismissReason.backdrop)}}r.reverseButtons?i.confirmButton.parentNode.insertBefore(i.cancelButton,i.confirmButton):i.confirmButton.parentNode.insertBefore(i.confirmButton,i.cancelButton);var w=function(e,t){for(var n=z(),o=0;o<n.length;o++){(e+=t)===n.length?e=0:-1===e&&(e=n.length-1);var r=n[e];if(O(r))return r.focus()}i.popup.focus()};X.keydownHandlerAdded&&(X.keydownTarget.removeEventListener("keydown",X.keydownHandler,{capture:X.keydownListenerCapture}),X.keydownHandlerAdded=!1),r.toast||(X.keydownHandler=function(e){return function(e,t){if(t.stopKeydownPropagation&&e.stopPropagation(),"Enter"!==e.key||e.isComposing)if("Tab"===e.key){for(var o=e.target||e.srcElement,r=z(),s=-1,u=0;u<r.length;u++)if(o===r[u]){s=u;break}w(s,e.shiftKey?-1:1),e.stopPropagation(),e.preventDefault()}else-1!==["ArrowLeft","ArrowRight","ArrowUp","ArrowDown","Left","Right","Up","Down"].indexOf(e.key)?document.activeElement===i.confirmButton&&O(i.cancelButton)?i.cancelButton.focus():document.activeElement===i.cancelButton&&O(i.confirmButton)&&i.confirmButton.focus():"Escape"!==e.key&&"Esc"!==e.key||!0!==m(t.allowEscapeKey)||c(a.DismissReason.esc);else if(e.target&&n.getInput()&&e.target.outerHTML===n.getInput().outerHTML){if(-1!==["textarea","file"].indexOf(t.input))return;a.clickConfirm(),e.preventDefault()}}(e,r)},X.keydownTarget=r.keydownListenerCapture?window:i.popup,X.keydownListenerCapture=r.keydownListenerCapture,X.keydownTarget.addEventListener("keydown",X.keydownHandler,{capture:X.keydownListenerCapture}),X.keydownHandlerAdded=!0),n.enableButtons(),n.hideLoading(),n.resetValidationError(),(r.input||r.footer||r.showCloseButton)&&B(document.body,y["toast-column"]);for(var C=["input","file","range","select","radio","checkbox","textarea"],x=void 0,A=0;A<C.length;A++){var L=y[C[A]],T=S(i.content,L);if(x=n.getInput(C[A])){for(var j in x.attributes)if(x.attributes.hasOwnProperty(j)){var _=x.attributes[j].name;"type"!==_&&"value"!==_&&x.removeAttribute(_)}for(var V in r.inputAttributes)x.setAttribute(V,r.inputAttributes[V])}T.className=L,r.inputClass&&B(T,r.inputClass),E(T)}var q=void 0;switch(r.input){case"text":case"email":case"password":case"number":case"tel":case"url":(x=S(i.content,y.input)).value=r.inputValue,x.placeholder=r.inputPlaceholder,x.type=r.input,P(x);break;case"file":(x=S(i.content,y.file)).placeholder=r.inputPlaceholder,x.type=r.input,P(x);break;case"range":var M=S(i.content,y.range),I=M.querySelector("input"),H=M.querySelector("output");I.value=r.inputValue,I.type=r.input,H.value=r.inputValue,P(M);break;case"select":var R=S(i.content,y.select);if(R.innerHTML="",r.inputPlaceholder){var D=document.createElement("option");D.innerHTML=r.inputPlaceholder,D.value="",D.disabled=!0,D.selected=!0,R.appendChild(D)}q=function(e){e.forEach(function(e){var t=s(e,2),n=t[0],o=t[1],i=document.createElement("option");i.value=n,i.innerHTML=o,r.inputValue.toString()===n.toString()&&(i.selected=!0),R.appendChild(i)}),P(R),R.focus()};break;case"radio":var N=S(i.content,y.radio);N.innerHTML="",q=function(e){e.forEach(function(e){var t=s(e,2),n=t[0],o=t[1],i=document.createElement("input"),a=document.createElement("label");i.type="radio",i.name=y.radio,i.value=n,r.inputValue.toString()===n.toString()&&(i.checked=!0);var u=document.createElement("span");u.innerHTML=o,u.className=y.label,a.appendChild(i),a.appendChild(u),N.appendChild(a)}),P(N);var t=N.querySelectorAll("input");t.length&&t[0].focus()};break;case"checkbox":var W=S(i.content,y.checkbox),U=n.getInput("checkbox");U.type="checkbox",U.value=1,U.id=y.checkbox,U.checked=Boolean(r.inputValue);var F=W.getElementsByTagName("span");F.length&&W.removeChild(F[0]),(F=document.createElement("span")).className=y.label,F.innerHTML=r.inputPlaceholder,W.appendChild(F),P(W);break;case"textarea":var K=S(i.content,y.textarea);K.value=r.inputValue,K.placeholder=r.inputPlaceholder,P(K);break;case null:break;default:d('Unexpected type of input! Expected "text", "email", "password", "number", "tel", "select", "radio", "checkbox", "textarea", "file" or "url", got "'+r.input+'"')}if("select"===r.input||"radio"===r.input){var Z=function(e){return q((t=e,n=[],"undefined"!=typeof Map&&t instanceof Map?t.forEach(function(e,t){n.push([t,e])}):Object.keys(t).forEach(function(e){n.push([e,t[e]])}),n));var t,n};h(r.inputOptions)?(a.showLoading(),r.inputOptions.then(function(e){n.hideLoading(),Z(e)})):"object"===e(r.inputOptions)?Z(r.inputOptions):d("Unexpected type of inputOptions! Expected object, Map or Promise, got "+e(r.inputOptions))}else-1!==["text","email","number","tel","textarea"].indexOf(r.input)&&h(r.inputValue)&&(a.showLoading(),E(x),r.inputValue.then(function(e){x.value="number"===r.input?parseFloat(e)||0:e+"",P(x),x.focus(),n.hideLoading()}).catch(function(e){d("Error in inputValue promise: "+e),x.value="",P(x),x.focus(),n.hideLoading()}));ve(r),r.toast||(m(r.allowEnterKey)?r.focusCancel&&O(i.cancelButton)?i.cancelButton.focus():r.focusConfirm&&O(i.confirmButton)?i.confirmButton.focus():w(-1,1):document.activeElement&&document.activeElement.blur()),i.container.scrollTop=0})}}),be=void 0;function we(){if("undefined"!=typeof window){"undefined"==typeof Promise&&d("This package requires a Promise library, please include a shim to enable it in this browser (See: https://github.com/sweetalert2/sweetalert2/wiki/Migration-from-SweetAlert-to-SweetAlert2#1-ie-support)");for(var e=arguments.length,t=Array(e),n=0;n<e;n++)t[n]=arguments[n];if(void 0===t[0])return d("SweetAlert2 expects at least 1 attribute!"),!1;be=this;var o=Object.freeze(this.constructor.argsToParams(t));Object.defineProperties(this,{params:{value:o,writable:!1,enumerable:!0}});var r=this._main(this.params);fe.promise.set(this,r)}}we.prototype.then=function(e,t){return fe.promise.get(this).then(e,t)},we.prototype.catch=function(e){return fe.promise.get(this).catch(e)},we.prototype.finally=function(e){return fe.promise.get(this).finally(e)},o(we.prototype,ye),o(we,le),Object.keys(ye).forEach(function(e){we[e]=function(){var t;if(be)return(t=be)[e].apply(t,arguments)}}),we.DismissReason=g,we.noop=function(){},we.version="7.25.6";var Ce,ke,xe=ee((Ce=we,ke=function(s){function u(){return t(this,u),a(this,(u.__proto__||Object.getPrototypeOf(u)).apply(this,arguments))}return i(u,Ce),n(u,[{key:"_main",value:function(e){return r(u.prototype.__proto__||Object.getPrototypeOf(u.prototype),"_main",this).call(this,o({},se,e))}}],[{key:"setDefaults",value:function(t){if(f(ae),!t||"object"!==(void 0===t?"undefined":e(t)))throw new TypeError("SweetAlert2: The argument for setDefaults() is required and has to be a object");ie(t),Object.keys(t).forEach(function(e){Ce.isValidParameter(e)&&(se[e]=t[e])})}},{key:"resetDefaults",value:function(){f(ae),se={}}}]),u}(),"undefined"!=typeof window&&"object"===e(window._swalDefaults)&&ke.setDefaults(window._swalDefaults),ke));return xe.default=xe}),"undefined"!=typeof window&&window.Sweetalert2&&(window.swal=window.sweetAlert=window.Swal=window.SweetAlert=window.Sweetalert2);