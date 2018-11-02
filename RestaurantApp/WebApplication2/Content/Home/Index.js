
$(document).ready(function () {
	var self = {};
	self.Menu = ko.mapping.fromJS(menu);
	ko.applyBindings(self);
});