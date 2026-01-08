mergeInto(LibraryManager.library, {
  IsWebGLTouchDevice: function() {
    if (typeof navigator !== 'undefined' && 'maxTouchPoints' in navigator) {
      return navigator.maxTouchPoints > 0;
    }
    return false;
  }
});
