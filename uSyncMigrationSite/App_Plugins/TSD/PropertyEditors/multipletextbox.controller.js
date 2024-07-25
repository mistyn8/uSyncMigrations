function MultipleTextBoxController($scope, $timeout) {

    var backspaceHits = 0;

    // Set the visible prompt to -1 to ensure it will not be visible
    $scope.promptIsVisible = "-1";

    $scope.sortableOptions = {
        axis: 'y',
        containment: 'parent',
        cursor: 'move',
        items: '> div.textbox-wrapper',
        tolerance: 'pointer'
    };

    if (!$scope.model.value) {
        $scope.model.value = [];
    }

    if (!$scope.model.duplicates) {
        $scope.model.duplicates = [];
    }


    // Add any fields that there isn't values for
    if ($scope.model.config.min > 0) {
        for (var i = 0; i < $scope.model.config.min; i++) {
            if ((i + 1) > $scope.model.value.length) {
                $scope.model.value.push({ value: "" });
            }
        }
    }

    $scope.paste = function (event, index) {
        event.preventDefault();
        var text = (event.clipboardData || window.clipboardData || event.originalEvent.clipboardData).getData('text');
        var lines = text.split(/\r?\n/).map(line => { return { value: line, hasFocus: false } });

        if (lines.length > 0) {
            lines = text.split(/,/).map(line => { return { value: line, hasFocus: false } });
        }

        if (lines.length > 0) {
            // merge with the current text
            var currentText = $scope.model.value[index].value;
            lines[0].value = currentText.substring(0, event.target.selectionStart) + lines[0].value;
            lines[lines.length - 1].value = lines[lines.length - 1].value + currentText.substring(event.target.selectionEnd);

            // clear selection
            event.target.selectionEnd = event.target.selectionStart;

            // remove focus from existing values
            $scope.model.value.forEach(value => value.hasFocus = false);

            // add all the lines to the value
            lines[lines.length - 1].hasFocus = true;
            $scope.model.value.splice(index, 1, ...lines);
        }
    }

    $scope.addRemoveOnKeyDown = function (event, index) {

        var txtBoxValue = $scope.model.value[index];

        event.preventDefault();

        switch (event.keyCode) {
            case 13:
                if ($scope.model.config.max <= 0 && txtBoxValue.value || $scope.model.value.length < $scope.model.config.max && txtBoxValue.value) {
                    var newItemIndex = index + 1;
                    $scope.model.value.splice(newItemIndex, 0, { value: "" });
                    // Focus on the newly added value
                    $scope.model.value[newItemIndex].hasFocus = true;
                }
                break;
            case 8:

                if ($scope.model.value.length > $scope.model.config.min) {
                    var remainder = [];

                    // Used to require an extra hit on backspace for the field to be removed
                    if (txtBoxValue.value === "") {
                        backspaceHits++;
                    } else {
                        backspaceHits = 0;
                    }

                    if (txtBoxValue.value === "" && backspaceHits === 2) {
                        for (var x = 0; x < $scope.model.value.length; x++) {
                            if (x !== index) {
                                remainder.push($scope.model.value[x]);
                            }
                        }

                        $scope.model.value = remainder;

                        var prevItemIndex = index - 1;

                        // Set focus back on false as the directive only watches for true
                        if (prevItemIndex >= 0) {
                            $scope.model.value[prevItemIndex].hasFocus = false;
                            $timeout(function () {
                                // Focus on the previous value
                                $scope.model.value[prevItemIndex].hasFocus = true;
                            });
                        }

                        backspaceHits = 0;
                    }
                }

                break;
            default:
        }
        validate();
    };

    $scope.add = function () {
        if ($scope.model.config.max <= 0 || $scope.model.value.length < $scope.model.config.max) {
            $scope.model.value.push({ value: "" });

            // Focus new value
            var newItemIndex = $scope.model.value.length - 1;
            $scope.model.value[newItemIndex].hasFocus = true;
        }
        validate();
    };

    $scope.remove = function (index) {
        // Make sure not to trigger other prompts when remove is triggered
        $scope.hidePrompt();

        var remainder = [];
        for (var x = 0; x < $scope.model.value.length; x++) {
            if (x !== index) {
                remainder.push($scope.model.value[x]);
            }
        }
        $scope.model.value = remainder;
    };

    $scope.showPrompt = function (idx, item) {

        var i = $scope.model.value.indexOf(item);

        // Make the prompt visible for the clicked tag only
        if (i === idx) {
            $scope.promptIsVisible = i;
        }
    };

    $scope.hidePrompt = function () {
        $scope.promptIsVisible = "-1";
    };

    $scope.model.displayDuplicates = function () {
        return $scope.model.duplicates.map(u => u.value).join(', ');
    }

    function validate() {

        if ($scope.multipleTextboxForm) {

            // find duplicates and retain the original idx
            $scope.model.duplicates = [];
            $scope.model.value.filter(function (element, index, array) {
                if (array.findIndex(e => e.value === element.value) !== index) {
                    if (element.value !== "") {
                        $scope.model.duplicates.push({ value: element.value, index: index });
                    }
                    return true;
                }
                return false;
            })

            // show error if we find duplicates
            $scope.multipleTextboxForm.duplicates.$setValidity("duplicates", !$scope.model.duplicates.length);

            // show the errors
            $scope.model.value.forEach(function (item, idx) {
                var uid = `item_${idx}`;

                if ($scope.model.duplicates.length) {
                    //duplicates
                    if (item.value !== '') {
                        $scope.multipleTextboxForm[uid].$setValidity('duplicate', $scope.model.duplicates.findIndex(e => e.index === idx) === -1);
                    }
                }

                // regex
                const regexp = new RegExp($scope.model.validation.pattern);
                if (item.value !== '') {
                    const regexpValid = regexp.test(item.value);
                    $scope.multipleTextboxForm[uid].$setValidity('valRegex', regexpValid);
                }

            })

            //show error if not enough items
            var invalid = $scope.model.validation.mandatory && !$scope.model.value.length
            $scope.multipleTextboxForm.mandatory.$setValidity("minCount", !invalid);
        }
    }

    $timeout(function () {
        validate();
    });

    // We always need to ensure we dont submit anything broken
    var unsubscribe = $scope.$on("formSubmitting", function (ev, args) {
        // Filter to items with values
        $scope.model.value = $scope.model.value.filter(el => el.value.trim() !== "") || [];
    });


    const sortItemsAction = {
        labelKey: "clipboard_sortItems",
        labelTokens: [],
        icon: "arrow-down",
        method: sortItems,
        isDisabled: !$scope.model.value.length
    };

    const deleteAllItemsAction = {
        labelKey: 'clipboard_labelForRemoveAllEntries',
        labelTokens: [],
        icon: 'trash',
        method: deleteAllItems,
        isDisabled: !$scope.model.value.length
    };

    const deleteDuplicatesAction = {
        labelKey: 'clipboard_labelForRemoveDuplicates',
        labelTokens: [],
        icon: 'trash-alt-2',
        method: deleteDuplicates,
        isDisabled: !$scope.model.value.length
    }

    function sortItems() {
        $scope.model.value.sort((a, b) => a.value.localeCompare(b.value));
    }
    function deleteAllItems() {
        $scope.model.value = [];
        validate();
    }

    function deleteDuplicates() {
        $scope.model.value = $scope.model.value.filter(function (element, index, array) {
            return array.findIndex(e => e.value === element.value) === index;
        })
        validate();
    }

    var propertyActions = [
        sortItemsAction,
        deleteAllItemsAction,
        deleteDuplicatesAction
    ];

    this.$onInit = function () {
        if ($scope.umbProperty) {
            $scope.umbProperty.setPropertyActions(propertyActions);
        }
    };

    // When the scope is destroyed we need to unsubscribe
    $scope.$on('$destroy', function () {
        unsubscribe();
    });
}

angular.module("umbraco").controller("Tsd.PropertyEditors.MultipleTextBoxController", MultipleTextBoxController);