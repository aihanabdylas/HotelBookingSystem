
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace HotelBookingSystem
{
    public partial class Form1 : Form
    {
        // GUI controls
        private TextBox txtGuestName = null!;
        private TextBox txtEmail = null!;
        private ComboBox cmbRoom = null!;
        private NumericUpDown numNights = null!;
        private Label lblTotal = null!;
        private DataGridView dgvRooms = null!;
        private DataGridView dgvBookings = null!;
        private List<Booking> bookings = new List<Booking>();

        // Room objects
        private Room[] rooms = Array.Empty<Room>();

        public Form1()
        {
            InitializeComponent();

            // Create actual room objects
            rooms = new Room[]
            {
                new StandardRoom("101", 100.00),
                new SuiteRoom("201", 250.00, 50.00)
            };

            BuildInterface();
        }

        private void BuildInterface()
        {
            // Main window settings
            this.Text = "Hotel Booking System";
            this.Size = new Size(1100, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);

            // HEADER
            Panel header = new Panel();
            header.Size = new Size(1100, 100);
            header.Location = new Point(0, 0);
            header.BackColor = Color.FromArgb(25, 45, 70);
            this.Controls.Add(header);

            Label title = new Label();
            title.Text = "HOTEL BOOKING SYSTEM";
            title.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.Location = new Point(35, 20);
            title.AutoSize = true;
            header.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "ROOM RESERVATION MANAGEMENT";
            subtitle.Font = new Font("Segoe UI", 9);
            subtitle.ForeColor = Color.LightGray;
            subtitle.Location = new Point(39, 62);
            subtitle.AutoSize = true;
            header.Controls.Add(subtitle);

            // GUEST INFORMATION
            Panel guestPanel = CreatePanel(35, 130, 480, 200);
            this.Controls.Add(guestPanel);

            AddSectionTitle(guestPanel, "Guest Information");

            AddLabel(guestPanel, "Guest Name:", 25, 65);
            txtGuestName = CreateTextBox(160, 60, 280);
            guestPanel.Controls.Add(txtGuestName);

            AddLabel(guestPanel, "Email Address:", 25, 120);
            txtEmail = CreateTextBox(160, 115, 280);
            guestPanel.Controls.Add(txtEmail);

            // ROOM SELECTION
            Panel roomPanel = CreatePanel(545, 130, 520, 200);
            this.Controls.Add(roomPanel);

            AddSectionTitle(roomPanel, "Room Selection");

            AddLabel(roomPanel, "Select Room:", 25, 65);

            cmbRoom = new ComboBox();
            cmbRoom.Location = new Point(160, 60);
            cmbRoom.Size = new Size(300, 30);
            cmbRoom.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbRoom.Items.Add("Standard Room - 101");
            cmbRoom.Items.Add("Suite Room - 201");

            cmbRoom.SelectedIndex = 0;
            cmbRoom.SelectedIndexChanged += UpdateTotal;

            roomPanel.Controls.Add(cmbRoom);

            AddLabel(roomPanel, "Number of Nights:", 25, 120);

            numNights = new NumericUpDown();
            numNights.Location = new Point(160, 115);
            numNights.Size = new Size(100, 30);
            numNights.Minimum = 1;
            numNights.Maximum = 365;
            numNights.Value = 1;
            numNights.ValueChanged += UpdateTotal;

            roomPanel.Controls.Add(numNights);

            // BOOKING SUMMARY
            Panel pricePanel = CreatePanel(35, 350, 480, 150);
            this.Controls.Add(pricePanel);

            AddSectionTitle(pricePanel, "Booking Summary");

            AddLabel(pricePanel, "Estimated Total:", 25, 70);

            lblTotal = new Label();
            lblTotal.Text = "$100.00";
            lblTotal.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(35, 120, 75);
            lblTotal.Location = new Point(190, 60);
            lblTotal.AutoSize = true;

            pricePanel.Controls.Add(lblTotal);

            // ROOM AVAILABILITY
            Panel availabilityPanel = CreatePanel(545, 350, 520, 300);
            this.Controls.Add(availabilityPanel);

            AddSectionTitle(availabilityPanel, "Room Availability");

            dgvRooms = new DataGridView();
            dgvRooms.Location = new Point(20, 60);
            dgvRooms.Size = new Size(480, 210);

            dgvRooms.AllowUserToAddRows = false;
            dgvRooms.AllowUserToDeleteRows = false;
            dgvRooms.ReadOnly = true;
            dgvRooms.RowHeadersVisible = false;

            dgvRooms.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvRooms.BackgroundColor = Color.White;

            dgvRooms.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvRooms.Columns.Add("RoomNumber", "Room No.");
            dgvRooms.Columns.Add("RoomType", "Room Type");
            dgvRooms.Columns.Add("Price", "Price/Night");
            dgvRooms.Columns.Add("Status", "Status");

            dgvRooms.Rows.Add("101", "Standard", "$100", "Available");
            dgvRooms.Rows.Add("201", "Suite", "$300", "Available");

            availabilityPanel.Controls.Add(dgvRooms);

            // CONFIRM BOOKING BUTTON
            Button btnConfirm = CreateButton(
                "Confirm Booking",
                35, 540, 225, 50,
                Color.FromArgb(35, 120, 75));

            btnConfirm.Click += ConfirmBooking;

            this.Controls.Add(btnConfirm);

            // CLEAR BUTTON
            Button btnClear = CreateButton(
                "Clear Form",
                290, 540, 225, 50,
                Color.FromArgb(90, 100, 115));

            btnClear.Click += ClearForm;

            this.Controls.Add(btnClear);
            
            // BOOKING HISTORY PANEL
            Panel historyPanel = CreatePanel(35, 680, 1030, 250);
            AddSectionTitle(historyPanel, "Booking History");

            Button btnCancel = CreateButton(
                "Cancel Booking",
                790, 12, 210, 38,
                Color.FromArgb(180, 55, 55));

            btnCancel.Click += CancelBooking;

            historyPanel.Controls.Add(btnCancel);

            this.Controls.Add(historyPanel);

            AddSectionTitle(historyPanel, "Booking History");

            dgvBookings = new DataGridView();

            dgvBookings.Location = new Point(20, 60);
            dgvBookings.Size = new Size(990, 170);

            dgvBookings.AllowUserToAddRows = false;
            dgvBookings.AllowUserToDeleteRows = false;
            dgvBookings.ReadOnly = true;
            dgvBookings.RowHeadersVisible = false;

            dgvBookings.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvBookings.BackgroundColor = Color.White;

            dgvBookings.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Create booking history columns
            dgvBookings.Columns.Add("BookingID", "Booking ID");
            dgvBookings.Columns.Add("GuestName", "Guest Name");
            dgvBookings.Columns.Add("Email", "Email");
            dgvBookings.Columns.Add("RoomNumber", "Room No.");
            dgvBookings.Columns.Add("RoomType", "Room Type");
            dgvBookings.Columns.Add("Nights", "Nights");
            dgvBookings.Columns.Add("Total", "Total Price");
            dgvBookings.Columns.Add("Status", "Booking Status");

            historyPanel.Controls.Add(dgvBookings);


            UpdateTotal(this, EventArgs.Empty);
        }

        // Create panels
        private Panel CreatePanel(
            int x, int y, int width, int height)
        {
            Panel panel = new Panel();

            panel.Location = new Point(x, y);
            panel.Size = new Size(width, height);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            return panel;
        }

        // Create section headings
        private void AddSectionTitle(Panel panel, string text)
        {
            Label label = new Label();

            label.Text = text;
            label.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(25, 45, 70);
            label.Location = new Point(20, 15);
            label.AutoSize = true;

            panel.Controls.Add(label);
        }

        // Create labels
        private void AddLabel(
            Panel panel, string text, int x, int y)
        {
            Label label = new Label();

            label.Text = text;
            label.Location = new Point(x, y);
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 10);

            panel.Controls.Add(label);
        }

        // Create textboxes
        private TextBox CreateTextBox(
            int x, int y, int width)
        {
            TextBox textbox = new TextBox();

            textbox.Location = new Point(x, y);
            textbox.Size = new Size(width, 30);
            textbox.Font = new Font("Segoe UI", 10);

            return textbox;
        }

        // Create buttons
        private Button CreateButton(
            string text,
            int x, int y,
            int width, int height,
            Color buttonColor)
        {
            Button button = new Button();

            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, height);
            button.BackColor = buttonColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(
                "Segoe UI", 10, FontStyle.Bold);

            return button;
        }

        // Calculate estimated price using Room objects
        private void UpdateTotal(object? sender, EventArgs e)
        {
            if (cmbRoom.SelectedIndex < 0)
                return;

            Room selectedRoom = rooms[cmbRoom.SelectedIndex];

            double total = selectedRoom.CalculateTotalPrice(
                (int)numNights.Value);

            lblTotal.Text = "$" + total.ToString("F2");
        }

        // Process booking
        private void ConfirmBooking(object? sender, EventArgs e)
        {
            // Validate guest name
            if (string.IsNullOrWhiteSpace(txtGuestName.Text))
            {
                MessageBox.Show(
                    "Please enter the guest name.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtGuestName.Focus();
                return;
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(txtEmail.Text)
                || !txtEmail.Text.Contains("@")
                || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            // Retrieve selected room
            Room selectedRoom = rooms[cmbRoom.SelectedIndex];

            // Create Guest object
            Guest guest = new Guest(
                txtGuestName.Text.Trim(),
                txtEmail.Text.Trim());

            // Create Booking object
            Booking booking = new Booking(
                guest,
                selectedRoom,
                (int)numNights.Value);

            // Process booking
            bool success = booking.ProcessBooking(out string message);

            if (success)
            {
                // Store the actual booking object
                bookings.Add(booking);
                // Update room availability in the table
                dgvRooms.Rows[cmbRoom.SelectedIndex]
                    .Cells["Status"].Value = "Booked";
                
                // Add successful booking to history
                int bookingID = dgvBookings.Rows.Count + 1;

                string roomType = cmbRoom.SelectedIndex == 0
                    ? "Standard"
                    : "Suite";

                dgvBookings.Rows.Add(
                    bookingID,
                    txtGuestName.Text.Trim(),
                    txtEmail.Text.Trim(),
                    selectedRoom.GetRoomNumber(),
                    roomType,
                    numNights.Value,
                    "$" + booking.TotalPrice.ToString("F2"),
                    "Active"
                );


                // Display booking confirmation
                MessageBox.Show(
                    message +
                    "\n\n" + selectedRoom.GetRoomDetails() +
                    "\nNights: " + numNights.Value +
                    "\nTotal Price: $" + booking.TotalPrice.ToString("F2"),
                    "Booking Confirmed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                // Display booking error
                MessageBox.Show(
                    message,
                    "Booking Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Clear guest information
        private void ClearForm(object? sender, EventArgs e)
        {
            txtGuestName.Clear();
            txtEmail.Clear();

            cmbRoom.SelectedIndex = 0;
            numNights.Value = 1;

            txtGuestName.Focus();
        }


    private void CancelBooking(object? sender, EventArgs e)
    {
        // Check whether a booking has been selected
        if (dgvBookings.SelectedRows.Count == 0)
        {
            MessageBox.Show(
                "Please select a booking from the Booking History table.",
                "No Booking Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        // Retrieve the selected booking's row index
        int selectedIndex =
            dgvBookings.SelectedRows[0].Index;

        // Retrieve the corresponding Booking object
        Booking selectedBooking = bookings[selectedIndex];

        // Check whether booking is already cancelled
        if (!selectedBooking.IsActive)
        {
            MessageBox.Show(
                "This booking has already been cancelled.",
                "Cancellation Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        // Ask user to confirm cancellation
        DialogResult result = MessageBox.Show(
            "Are you sure you want to cancel this booking?",
            "Confirm Cancellation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.No)
        {
            return;
        }

        // Cancel the booking
        bool success = selectedBooking.CancelBooking(
            out string message);

        if (success)
        {
            // Update booking history status
            dgvBookings.Rows[selectedIndex]
                .Cells["Status"].Value = "Cancelled";

            // Retrieve the room number
            string roomNumber =
                dgvBookings.Rows[selectedIndex]
                .Cells["RoomNumber"].Value.ToString()!;

            // Update room availability
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].GetRoomNumber() == roomNumber)
                {
                    dgvRooms.Rows[i]
                        .Cells["Status"].Value = "Available";

                    break;
                }
            }

            // Display cancellation confirmation
            MessageBox.Show(
                message,
                "Booking Cancelled",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(
                message,
                "Cancellation Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
}