﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelState State { get; set; }
        public CustomerInParcel OtherSide { get; set; }

        public override string ToString()
        {
            string str = $"Parcel {Id}:";
            str += $"\nWeight - {Weight}";
            str += $"\n Priority - {Priority}";
            str += $"\nOther side - {OtherSide}";
            return str;
        }
    }
}