function setNotification(title) {
    var dateTimeMs = document.getElementById("datePickerForNotification").valueAsNumber;
    var alarm = new Date(dateTimeMs);
    var alarmTime = new Date(alarm.getUTCFullYear(), alarm.getUTCMonth(), alarm.getUTCDate(), alarm.getUTCHours(), alarm.getUTCMinutes(), alarm.getUTCSeconds());
    setTimeout(function () { notifyWithPopup(title); }, alarmTime.getTime() - (new Date()).getTime());
}
function notifyWithPopup(title) {
    window.open('popup?title=' + title, 'new window', 'width=350, height=150, dependent=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=no, toolbar=no, status=no, center=yes, overflow-y=hidden');
}

