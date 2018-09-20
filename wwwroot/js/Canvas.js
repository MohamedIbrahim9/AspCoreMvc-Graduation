$(window).ready(function () {
    var ImageWidth = $('#my-image').width();
    var ImageHeight = $('#my-image').height();
    var canvas = window._canvas = new fabric.Canvas('c');
    console.log($("#my-image"));
    
    setTimeout(function () {
        // Get the width here
        var ImageWidth = $("#my-image").width();
        var ImageHeight = $("#my-image").height();
        canvas.setWidth(ImageWidth);
        canvas.setHeight(ImageHeight);
        var CanvasWidth = $("#c").width();
        var CanvasHeight = $("#c").height();
        console.log("Canvas width : " + CanvasWidth + "\n" + "Canvas height : " + CanvasHeight);
        console.log("Image width : " + ImageWidth + "\n" + "Image height : " + ImageHeight);
    }, 1000);
   
    var circle1 = new fabric.Circle({
        radius: 7,
        fill: 'red',
        stroke: '#333333',
        strokeWidth: 1,
        left: 100,
        top: 100,
        selectable: true,
        hasBorders: false,
        hasControls: false,
        originX: 'center',
        originY: 'center',
        opacity: .9,
    });
    var circle2 = new fabric.Circle({
        radius: 7,
        fill: 'grey',
        stroke: '#333333',
        strokeWidth: 1,
        left: 400,
        top: 100,
        selectable: true,
        hasBorders: false,
        hasControls: false,
        originX: 'center',
        originY: 'center',
        opacity: .9,
    });
    var circle3 = new fabric.Circle({
        radius: 7,
        fill: 'grey',
        stroke: '#333333',
        strokeWidth: 1,
        left: 400,
        top: 400,
        selectable: true,
        hasBorders: false,
        hasControls: false,
        originX: 'center',
        originY: 'center',
        opacity: .9,
    });
    var circle4 = new fabric.Circle({
        radius: 7,
        fill: 'grey',
        stroke: '#333333',
        strokeWidth: 1,
        left: 100,
        top: 400,
        selectable: true,
        hasBorders: false,
        hasControls: false,
        originX: 'center',
        originY: 'center',
        opacity: .9,
    });
    var rect = new fabric.Rect({
        left: 100,
        top: 100,
        fill: 'red',
        width: 20,
        height: 20,
        angle: 45
    });
    var point1 = [circle1.left, circle1.top, circle2.left, circle2.top];
    var line1 = new fabric.Line(point1, {
        strokeWidth: 2,
        fill: '#999999',
        stroke: '#999999',
        class: 'line',
        originX: 'center',
        originY: 'center',
        selectable: false,
        hasBorders: false,
        hasControls: false,
        evented: false
    });
    var count = 0;
    var poly;
    $("#draw-poly").click(function () {
        if (count == 0) {
            DisabeCircle();
            poly = new fabric.Polygon([{ x: circle1.left, y: circle1.top },
            { x: circle2.left, y: circle2.top },
            { x: circle3.left, y: circle3.top },
            { x: circle4.left, y: circle4.top }], {
                    stroke: '#333333',
                    strokeWidth: 1,
                    fill: 'red',
                    opacity: 0.4,
                    selectable: false,
                    hasBorders: false,
                    hasControls: false,
                    evented: false
                });
            canvas.add(poly);
            $("#submit").removeAttr("disabled");
            $("#reset").removeAttr("disabled");
        }
        count++;
    });
    $("#reset").click(function () {
        $("#submit").attr("disabled", true);
        count = 0;
        EnableCircle();
        ResetCircles();
        canvas.remove(poly);
        AddCircles();
    });
    var AddCircles = function () {
        canvas.add(circle1);
        canvas.add(circle2);
        canvas.add(circle3);
        canvas.add(circle4);
    };
    var EnableCircle = function () {
        circle1.selectable = true;
        circle2.selectable = true;
        circle3.selectable = true;
        circle4.selectable = true;
    };
    var DisabeCircle = function () {
        circle1.selectable = false;
        circle2.selectable = false;
        circle3.selectable = false;
        circle4.selectable = false;
    };
    var ResetCircles = function () {
        circle1.left = 100; circle1.top = 100;
        circle2.left = 400; circle2.top = 100;
        circle3.left = 400; circle3.top = 400;
        circle4.left = 100; circle4.top = 400;
    };

    circle1.on('modified', function () {
        $("#reset").removeAttr("disabled");
    });
    circle2.on('modified', function () {
        $("#reset").removeAttr("disabled");
    });
    circle3.on('modified', function () {
        $("#reset").removeAttr("disabled");
    });
    circle4.on('modified', function () {
        $("#reset").removeAttr("disabled");
    });

    var slider = document.getElementById("myRange");
    var output = document.getElementById("demo");
    output.innerHTML = slider.value;
    slider.oninput = function () {
        output.innerHTML = this.value;
    }
    var ratio;
    $(function () {
        $('#my-image').each(function () {
            var maxWidth = 900; // Max width for the image
            //var maxHeight = 200;    // Max height for the image
            //var maxratio=maxHeight/maxWidth;
            var width = $(this).width();    // Current image width
            var height = $(this).height();  // Current image height
            var imgElement = document.getElementById('my-image');
            //var imgInstanc;
            //var ImageRatio=height/width;
            // Check if the current width is larger than the max
            if (width > maxWidth) {
                ratio = maxWidth / width;   // get ratio for scaling image
                $(this).attr('width', maxWidth); // Set new width
                $(this).attr('height', height * ratio); // Scale height based on ratio
                canvas.setWidth(maxWidth);
                canvas.setHeight(height * ratio);
                var imgInstance = new fabric.Image(imgElement, {
                    left: 0,
                    top: 0,
                    selectable: false,
                    opacity: 0.85
                });
                canvas.add(imgInstance);
                AddCircles();
            }
            else {
                ratio = 1;
                imgInstance = new fabric.Image(imgElement, {
                    left: 0,
                    top: 0,
                    selectable: false,
                    opacity: 0.85
                });
                canvas.add(imgInstance);
                AddCircles();
            }
        });
    });

    $("#submit").click(function () {
        var data = {
            TopLeft: {
                X: circle1.left / ratio,
                Y: circle1.top / ratio
            },
            TopRight: {
                X: circle2.left / ratio,
                Y: circle2.top / ratio
            },
            BottomRight: {
                X: circle3.left / ratio,
                Y: circle3.top / ratio
            },
            BottomLeft: {
                X: circle4.left / ratio,
                Y: circle4.top / ratio
            },
            Precision: Number(output.innerHTML),
        };
        //Sample 
        //JSON.stringify(data)
        $.ajax({
            url: 'https://localhost:44329/ObjFiles/CanvasDrawer',
            type: "POST",
            data: data,
            //contentType: "application/json; charset=utf-8",
            //crossDomain:true,
             //dataType: 'json',
            //traditional:true,
            //processData:true,
            success: function (data) {
                alert("data Saved");
            },
            done: function () {
                console.log("data Saved");
            },
            error: function (err) {
                alert("data Not Saved");
            }
        });
        console.log(data);
    });
});
