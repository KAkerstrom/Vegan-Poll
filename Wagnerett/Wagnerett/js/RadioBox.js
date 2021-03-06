﻿var RadioGroups = {};

function RadioBox(group, name, label) {
    if (!(group in RadioGroups)) {
        RadioGroups[group] = {};
    }

    var tr = $('<div class="RadioBox">');

    tr.attr('id', name);

    var button = $('<div class="Button"><div class="Dot"></div></div>');
    button[0].isSelected = false;
    button[0].group = group;
    button[0].radioName = name;

    tr.append(button);
    tr.append($('<div class="Label">').append(label));

    button.on('click', function () {
        this.Select();
    });

    tr.children('.Label').on('click', function () {
        button[0].Select();
    });

    button[0].Select = function () {
        var gr = RadioGroups[this.group];

        this.isSelected = true;//!this.isSelected;

        for (var key in gr) {
            if (key != this.radioName) {
                gr[key].isSelected = false;

                $(gr[key]).children('.Dot').animate({
                    opacity: '0.0',
                    width: '0px',
                    height: '0px'
                }, 150);
            }
        }

        button.children('.Dot').animate({
            opacity: '1.0',
            width: '15px',
            height: '15px'
        }, 150);
    }

    RadioGroups[group][name] = button[0];

    return $('<div class="Answer">').append(tr);
}

function RadioSelected(group) {
    if (group in RadioGroups) {
        var cg = RadioGroups[group];

        for (var key in cg) {
            if (cg[key].isSelected)
                return key;
        }
    }

    return null;
}