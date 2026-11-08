#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

void solve();

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);

    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}

void solve()
{
    int n;
    cin >> n;
    vector<int> a(n,0);
    vector<int> b(n,0);
    rep(i,0,n) cin>>a[i];
    rep(i,0,n) cin>>b[i];

    int me = INT_MAX, ix=-1;
    rep(i,0,n)
    {
        int x = (a[0]+b[i]) % n;
        if(x < me)
        {
            ix = i;
            me = x;
        }
    }

    vector<int> c(n,INT_MAX);
    vector<int> cc(n,0);
    rep(i,0,n)
    {
        int x = (a[0]+b[i]) % n;
        if(x == me)
        {
            bool replace = false;

            rep(j,0,n)
            {
                cc[j] = (a[j] + b[(i+j) % n]) % n;
            }

            int k=0;
            while(cc[k] == c[k]) k++;
            if(cc[k] < c[k])
            {
                rep(j,0,n) c[j] = cc[j];
            }
        }
    }

    rep(i,0,n) cout << c[i] << " ";
    cout<<"\n";
}


