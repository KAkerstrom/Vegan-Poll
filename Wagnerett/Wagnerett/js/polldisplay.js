var RadioGroups = {};

$(document).ready(function () {
    $(".PollBox").append(RadioBox('gr', 'yes', 'Yes'));
    $(".PollBox").append(RadioBox('gr', 'no', 'No'));
    $(".PollBox").append(RadioBox('gr', 'maybe', 'Maybe So'));
});

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
        var gr = RadioGroups[this.group];

        this.isSelected = true;//!this.isSelected;
        
        for (var key in gr) {
            if (key != this.radioName) {
                gr[key].isSelected = false;

                $(gr[key]).children('.Dot').animate({
                    opacity: '0.0',
                    width: '0%',
                    height: '0%'
                }, 150);
            }
        }
        
        button.children('.Dot').animate({
            opacity: '1.0',
            width: '70%',
            height: '70%'
        }, 150);
    });
    
    RadioGroups[group][name] = button[0];

    return $('<div class="Answer">').append(tr);
}