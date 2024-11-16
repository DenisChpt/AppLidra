window.drawPieChart = function (data) {
    const ctx = document.getElementById("pieChart").getContext("2d");
    new Chart(ctx, {
        type: "pie",
        data: {
            labels: data.labels,
            datasets: [{
                data: data.values,
                backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56"],
            }]
        }
    });
};

window.drawTimeGraph = function (data) {
    const ctx = document.getElementById("timeGraph").getContext("2d");
    new Chart(ctx, {
        type: "line",
        data: {
            labels: data.dates,
            datasets: [{
                data: data.amounts,
                borderColor: "#36A2EB",
                fill: false,
            }]
        }
    });
};
