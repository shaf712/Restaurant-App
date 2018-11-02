
$(document).ready(function () {
	var self = {};
	self.Breakfast = ko.mapping.fromJS(Breakfast);
	self.Lunch = ko.mapping.fromJS(Lunch);
	self.Dinner = ko.mapping.fromJS(Dinner);
	ko.applyBindings(self);
});